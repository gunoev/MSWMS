using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json.Serialization;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.IdentityModel.Tokens;
using MSWMS.Entities;
using MSWMS.Entities.External;
using MSWMS.Hubs;
using MSWMS.Infrastructure;
using MSWMS.Jobs;
using MSWMS.Models;
using MSWMS.Repositories;
using MSWMS.Services;
using MSWMS.Services.Interfaces;
using Serilog;
using Serilog.Events;
using OpenTelemetry.Metrics;
using Quartz;
using Serilog.Sinks.Grafana.Loki;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.AspNetCore.Hosting.Diagnostics", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.AspNetCore.Routing.EndpointMiddleware", LogEventLevel.Warning)
    .Filter.ByExcluding(logEvent =>
        logEvent.Properties.TryGetValue("RequestPath", out var requestPath) &&
        requestPath.ToString().Contains("/metrics"))
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .Enrich.FromLogContext()
    .Filter.ByExcluding(logEvent =>
        logEvent.Properties.TryGetValue("RequestPath", out var requestPath) &&
        requestPath.ToString().Contains("/metrics"))
    .WriteTo.GrafanaLoki("http://localhost:3100",
        labels: new List<LokiLabel>
    {
        new() { Key = "job", Value = "MSWMS" },
        new() { Key = "service_name", Value = "MSWMS" }
    }));

builder.Services.AddOpenTelemetry()
    .WithMetrics(metrics =>
    {
        metrics.AddAspNetCoreInstrumentation()
            .AddRuntimeInstrumentation()
            .AddProcessInstrumentation()
            .AddPrometheusExporter();
    });

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Игнорировать циклические ссылки при генерации JSON
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });
builder.WebHost.ConfigureKestrel(options =>
{
    options.ConfigureHttpsDefaults(httpsOptions =>
    {
        if (builder.Environment.IsDevelopment())
        {
            httpsOptions.ServerCertificate = new X509Certificate2("aspnetapp_dev.pfx");   
        }
        else
        {
            httpsOptions.ServerCertificate = new X509Certificate2("aspnetapp.pfx", "123123");   
        }
    });
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(connectionString));
    
    builder.Services.AddDbContext<ExternalReadOnlyContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("ExternalDb"), o => o.UseCompatibilityLevel(120)));
    
    builder.Services.AddDbContext<DCXWMSContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DCXDb"), o => o.UseCompatibilityLevel(120)));

}
else
{
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(connectionString));  
    
    builder.Services.AddDbContext<ExternalReadOnlyContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("ExternalDb"), o => o.UseCompatibilityLevel(120)));
    
    builder.Services.AddDbContext<DCXWMSContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DCXDb"), o => o.UseCompatibilityLevel(120)));

}

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = "MultiAuth"; 
        options.DefaultChallengeScheme = "MultiAuth";
        options.DefaultScheme = "MultiAuth";
        options.DefaultForbidScheme = "MultiAuth";
    })
    .AddCookie(options => 
    {
        options.Cookie.Name = "MSWMS.Auth";
        options.Cookie.HttpOnly = true;
        options.ExpireTimeSpan = TimeSpan.FromDays(1);
        options.LoginPath = "/api/Auth/login";
        options.Cookie.SameSite = SameSiteMode.Lax;
        options.AccessDeniedPath = "/api/Admin/login";
        options.SlidingExpiration = true;
        options.Events = new CookieAuthenticationEvents
        {
            OnRedirectToLogin = context =>
            {
                context.Response.StatusCode = 403;
                context.Response.ContentType = "application/json";
            
                return context.Response.WriteAsync(
                    System.Text.Json.JsonSerializer.Serialize(new { message = "Authorization required" })
                );
            },
            OnRedirectToAccessDenied = context =>
            {
                context.Response.StatusCode = 403;
                context.Response.ContentType = "application/json";
            
                return context.Response.WriteAsync(
                    System.Text.Json.JsonSerializer.Serialize(new { message = "Access denied" })
                );
            }
        };

    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT key not found in configuration")
                )
            )
        };
        options.Events = new JwtBearerEvents
        {
            OnChallenge = context =>
            {
                // Предотвращаем стандартное поведение
                context.HandleResponse();
            
                // Устанавливаем код 403 для неавторизованных запросов
                context.Response.StatusCode = 403;
                context.Response.ContentType = "application/json";
            
                return context.Response.WriteAsync(
                    System.Text.Json.JsonSerializer.Serialize(new { message = "Access denied" })
                );
            },
            OnForbidden = context =>
            {
                context.Response.StatusCode = 403;
                context.Response.ContentType = "application/json";
            
                return context.Response.WriteAsync(
                    System.Text.Json.JsonSerializer.Serialize(new { message = "No permissions" })
                );
            }
        };

    })
// Добавляем политику, которая поддерживает и JWT, и куки
    .AddPolicyScheme("MultiAuth", "JWT or Cookie", options =>
    {
        options.ForwardDefaultSelector = context =>
        {
            // Проверяем, есть ли JWT токен в заголовке
            string authorization = context.Request.Headers["Authorization"];
            if (!string.IsNullOrEmpty(authorization) && authorization.StartsWith("Bearer "))
                return JwtBearerDefaults.AuthenticationScheme;
            
            // Иначе используем куки
            return CookieAuthenticationDefaults.AuthenticationScheme;
        };
    });

builder.Services.AddAuthorization(options =>
{
    MSWMS.Infrastructure.Authorization.Policies.AddPolicies(options);
});



builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<BoxService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<ScanHub>();
builder.Services.AddScoped<IScanService, ScanService>();
builder.Services.AddScoped<ISalesPriceUpdater, SalesPriceUpdater>();

// Swagger конфигурация
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Регистрация сервиса аутентификации
builder.Services.AddScoped<IAuthService, AuthService>();

// Добавление пакета BCrypt для хеширования паролей
builder.Services.AddSingleton<BCrypt.Net.BCrypt>();
builder.Services.AddSignalR();

builder.Services.AddQuartz(q =>
{
    var jobKey = new JobKey(nameof(UpdatePricesJob));

    q.AddJob<UpdatePricesJob>(opts => opts.WithIdentity(jobKey));

    // Каждый день в 07:00 (cron Quartz: секунды минуты часы день-месяц месяц день-недели)
    q.AddTrigger(opts => opts
        .ForJob(jobKey)
        .WithIdentity($"{nameof(UpdatePricesJob)}-trigger")
        .WithCronSchedule("0 0 7 * * ?"));
});

builder.Services.AddQuartzHostedService(opts =>
{
    opts.WaitForJobsToComplete = true;
});

var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", corsBuilder =>
    {
        {
            corsBuilder
                .WithOrigins(allowedOrigins)
                .AllowAnyMethod()
                .AllowAnyHeader() 
                .AllowCredentials();
        }
    });
});

var mapperConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new MappingProfile());
}, new LoggerFactory());

IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);


var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var dbContext = services.GetRequiredService<AppDbContext>();
    var authService = services.GetRequiredService<IAuthService>();
    
    await dbContext.Database.MigrateAsync();
    
    await DataSeeder.SeedDefaultLocation(dbContext);
    await DataSeeder.SeedRoles(dbContext);
    await DataSeeder.SeedAdminUser(dbContext, authService);
}


app.UseSwagger();
app.UseSwaggerUI();


app.UseDefaultFiles();
app.UseStaticFiles();

app.MapFallbackToFile("index.html");

app.MapHub<ScanHub>("/api/scanhub");

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
//app.Urls.Add("http://0.0.0.0:5262");
app.Urls.Add("https://0.0.0.0:5262");

app.MapPrometheusScrapingEndpoint();

app.Run();
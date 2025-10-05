using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MSWMS.Entities;
using MSWMS.Infrastructure;
using MSWMS.Repositories;
using MSWMS.Services;
using MSWMS.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Регистрация AppDbContext
builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlite("Data Source=mswms.db")
);

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
                    builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT ключ не настроен")
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
builder.Services.AddScoped<IScanService, ScanService>();

// Swagger конфигурация
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Регистрация сервиса аутентификации
builder.Services.AddScoped<IAuthService, AuthService>();

// Добавление пакета BCrypt для хеширования паролей
builder.Services.AddSingleton<BCrypt.Net.BCrypt>();

// Добавьте CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        {
            builder
                .WithOrigins("http://localhost:5173", "http://localhost:5262")
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
    
    await DataSeeder.SeedDefaultLocation(dbContext);
    await DataSeeder.SeedRoles(dbContext);
    await DataSeeder.SeedAdminUser(dbContext, authService);
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
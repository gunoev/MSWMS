using Microsoft.EntityFrameworkCore;
using MSWMS.Entities;
using MSWMS.Repositories;
using MSWMS.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Регистрация AppDbContext
builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseInMemoryDatabase(builder.Configuration.GetConnectionString("TestConnection"))
);

builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<BoxService>();
builder.Services.AddScoped<ScanService>();
// Swagger конфигурация
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
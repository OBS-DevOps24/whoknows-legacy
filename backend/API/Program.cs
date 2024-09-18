using API.Data;
using API.Interfaces;
using API.Repositories;
using API.Services;
using dotenv.net;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Interfaces and implementations
builder.Services.AddScoped<IPageRepository, PageRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Load configuration from .env file
DotEnv.Load();
string connectionString = Environment.GetEnvironmentVariable("ConnectionString")
    ?? throw new InvalidOperationException("Connection string not found.");

// Configure the database
builder.Services.AddDbContext<DataContext>(options => 
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
        .LogTo(Console.WriteLine, LogLevel.Information)
        .EnableDetailedErrors());

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

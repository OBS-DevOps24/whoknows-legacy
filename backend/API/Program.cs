using API.Data;
using API.Interfaces;
using API.Repositories;
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

// Load configuration from .env file
DotEnv.Load();

string server = Environment.GetEnvironmentVariable("Server")
    ?? throw new InvalidOperationException("Server not found.");
string port = Environment.GetEnvironmentVariable("Port")
    ?? throw new InvalidOperationException("Port not found.");
string database = Environment.GetEnvironmentVariable("Database")
    ?? throw new InvalidOperationException("Database not found.");
string user = Environment.GetEnvironmentVariable("User");
string password = Environment.GetEnvironmentVariable("Pwd");

string connectionString = $"server={server};port={port};database={database};user={user};password={password}";

Console.WriteLine($"Connection string: {connectionString}");
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

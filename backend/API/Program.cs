using API.Data;
using API.Interfaces;
using API.Repositories;
using API.Services;
using dotenv.net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// CORS SETTINGS => Allow any origin, header, and method
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(MyAllowSpecificOrigins,
        policy =>
        {
            policy.SetIsOriginAllowed(origin => origin.StartsWith("http://localhost:"))
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
});


// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Interfaces and implementations
builder.Services.AddScoped<IPageRepository, PageRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPageService, PageService>();
builder.Services.AddScoped<IWeatherService, WeatherService>();
builder.Services.AddScoped<IGeocodingService, GeocodingService>();

// 
builder.Services.AddHttpClient();

// Authentication configuration
builder.Services.AddAuthentication(cfg => {
    cfg.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    cfg.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    cfg.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x => {
    x.RequireHttpsMetadata = false;
    x.SaveToken = false;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8
            .GetBytes(builder.Configuration["JWT_Secret"])
        ),
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = TimeSpan.Zero
    };
    x.Events = new JwtBearerEvents
    {
        OnTokenValidated = async context =>
        {
            var redisService = context.HttpContext.RequestServices.GetRequiredService<IRedisService>();
            var token = context.SecurityToken as JwtSecurityToken;
            if (token != null && await redisService.IsBlacklistedAsync(token.RawData))
            {
                context.Fail("Token is invalid.");
            }
        }
    };
});

// Redis configuration
builder.Services.AddSingleton<IRedisService, RedisService>();
builder.Services.AddStackExchangeRedisCache(options => { options.Configuration = builder.Configuration["Redis"]; });

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

app.UseCors(MyAllowSpecificOrigins);

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await app.RunAsync();

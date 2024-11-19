using API.Data;
using API.Interfaces;
using API.Repositories;
using API.Services;
using API.Middleware;
using dotenv.net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Prometheus;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;

var builder = WebApplication.CreateBuilder(args);

// Load configuration from .env file
DotEnv.Load();
builder.Configuration.AddEnvironmentVariables();

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

// Add OpenTelemetry metrics
builder.Services.AddOpenTelemetry()
    .ConfigureResource(resource => resource
        .AddService(serviceName: "whoknows-api"))
    .WithMetrics(metrics => metrics
        .AddAspNetCoreInstrumentation()
        .AddRuntimeInstrumentation()
        .AddHttpClientInstrumentation()
        .AddProcessInstrumentation()
        .AddPrometheusExporter());
// Configure Prometheus metrics endpoint
builder.Services.Configure<PrometheusExporterOptions>(options =>
{
    options.ScrapeEndpointPath = "/api/metrics";
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
var jwtSecret = Environment.GetEnvironmentVariable("JWT_KEY")
    ?? throw new InvalidOperationException("JWT_KEY not found.");
builder.Services.AddAuthentication(cfg =>
{
    cfg.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    cfg.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    cfg.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = false;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8
            .GetBytes(jwtSecret)
        ),
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = TimeSpan.Zero
    };
    x.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            context.Token = context.Request.Cookies["token"];
            return Task.CompletedTask;
        }
    };
});

// Redis configuration
var redisConnectionString = Environment.GetEnvironmentVariable("Redis")
    ?? throw new InvalidOperationException("Redis connection string not found.");
builder.Services.AddSingleton<IRedisService, RedisService>();
builder.Services.AddStackExchangeRedisCache(options => { options.Configuration = redisConnectionString; });

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

// Token blacklist middleware
app.UseTokenBlacklistMiddleware();

// Logging middleware to catch 415 errors
app.Use(async (context, next) =>
{
    // Log for auth endpoints
    if (context.Request.Path.StartsWithSegments("/api/register") ||
        context.Request.Path.StartsWithSegments("/api/login"))
    {
        var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();

        await next();

        // Log if it was a 415 error
        if (context.Response.StatusCode == 415)
        {
            logger.LogError(
                "415 Unsupported Media Type Error - {Method} {Path} - Content-Type: {ContentType}",
                context.Request.Method,
                context.Request.Path,
                context.Request.ContentType ?? "no content type"
            );
        }
    }
    else
    {
        await next();
    }
});

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Prometheus metrics endpoint
app.UseOpenTelemetryPrometheusScrapingEndpoint();

await app.RunAsync();

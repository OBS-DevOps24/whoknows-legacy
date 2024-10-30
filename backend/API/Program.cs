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
        },
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

// Logging middleware to catch 415 errors
app.Use(async (context, next) =>
{
    var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();

    // Log request details
    logger.LogInformation(
        "Request: {Method} {Path} - Content-Type: {ContentType}, Content-Length: {ContentLength}",
        context.Request.Method,
        context.Request.Path,
        context.Request.ContentType ?? "no content type",
        context.Request.ContentLength ?? 0
    );

    // Only log body for specific endpoints and methods
    if (context.Request.Path.StartsWithSegments("/api/register") || 
        context.Request.Path.StartsWithSegments("/api/login"))
    {
        try
        {
            context.Request.EnableBuffering();
            var body = await new StreamReader(context.Request.Body).ReadToEndAsync();
            logger.LogInformation("Request body: {body}", body);
            context.Request.Body.Position = 0;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error reading request body");
        }
    }

    await next();
});

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await app.RunAsync();

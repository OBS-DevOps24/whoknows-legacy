using API.Interfaces;
using System.IdentityModel.Tokens.Jwt;

namespace API.Middleware
{
    public class TokenBlacklistMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IRedisService _redisService;

        public TokenBlacklistMiddleware(RequestDelegate next, IRedisService redisService)
        {
            _next = next;
            _redisService = redisService;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var token = context.Request.Cookies["token"];

            if (!string.IsNullOrEmpty(token))
            {
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

                if (jsonToken != null)
                {
                    var jti = jsonToken.Claims.FirstOrDefault(claim => claim.Type == "jti")?.Value;

                    if (!string.IsNullOrEmpty(jti))
                    {
                        var isBlacklisted = await _redisService.IsBlacklistedAsync(jti);
                        if (isBlacklisted)
                        {
                            context.Response.StatusCode = 401;
                            await context.Response.WriteAsync("Token has been revoked");
                            return;
                        }
                    }
                }
            }

            await _next(context);
        }
    }

    public static class TokenBlacklistMiddlewareExtensions
    {
        public static IApplicationBuilder UseTokenBlacklistMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<TokenBlacklistMiddleware>();
        }
    }
}
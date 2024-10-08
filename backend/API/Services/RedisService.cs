using API.Interfaces;
using StackExchange.Redis;

namespace API.Services
{
    public class RedisService : IRedisService
    {
        private readonly ConnectionMultiplexer _redis;

        public RedisService(IConfiguration configuration)
        {
            var redisConnection = configuration["Redis"];
            if (string.IsNullOrEmpty(redisConnection))
            {
                throw new ArgumentException("Redis connection string is not set.");
            }
            _redis = ConnectionMultiplexer.Connect(redisConnection);
        }

        public async Task AddToBlacklistAsync(string jti, TimeSpan ttl)
        {
            var db = _redis.GetDatabase();
            await db.StringSetAsync(jti, "blacklisted", ttl);
        }

        public async Task<bool> IsBlacklistedAsync(string jti)
        {
            var db = _redis.GetDatabase();
            return await db.KeyExistsAsync(jti);
        }
    }
}

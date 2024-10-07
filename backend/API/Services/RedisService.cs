using API.Interfaces;
using StackExchange.Redis;

namespace API.Services
{
    public class RedisService : IRedisService
    {
        private readonly ConnectionMultiplexer _redis;

        public RedisService(IConfiguration configuration)
        {
            _redis = ConnectionMultiplexer.Connect(configuration.GetConnectionString("Redis"));
        }

        public async Task AddToBlacklistAsync(string token, TimeSpan ttl)
        {
            var db = _redis.GetDatabase();
            await db.StringSetAsync(token, "blacklisted", ttl);
        }

        public async Task<bool> IsBlacklistedAsync(string token)
        {
            var db = _redis.GetDatabase();
            return await db.KeyExistsAsync(token);
        }
    }
}

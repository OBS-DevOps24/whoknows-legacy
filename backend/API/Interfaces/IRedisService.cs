namespace API.Interfaces
{
    public interface IRedisService
    {
        Task AddToBlacklistAsync(string jti, TimeSpan ttl);
        Task<bool> IsBlacklistedAsync(string jti);
    }
}

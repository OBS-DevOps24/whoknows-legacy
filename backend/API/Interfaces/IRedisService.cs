namespace API.Interfaces
{
    public interface IRedisService
    {
        Task AddToBlacklistAsync(string token, TimeSpan ttl);
        Task<bool> IsBlacklistedAsync(string token);
    }
}

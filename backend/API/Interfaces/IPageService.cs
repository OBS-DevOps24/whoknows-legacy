using API.Models;

namespace API.Interfaces
{
    public interface IPageService
    {
        Task<List<Page>> GetByContent(string? q, string? lang);
    }
}

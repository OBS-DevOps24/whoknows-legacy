using API.Models;

namespace API.Interfaces
{
    public interface IPageRepository
    {
        Task<List<Page>> GetByContent(string? q, string? lang);
    }
}

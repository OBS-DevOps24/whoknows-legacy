using API.Models;

namespace API.Interfaces
{
    public interface IPageRepository
    {
        Task<Page> GetByTitleAsync(string title);
    }
}

using API.Interfaces;
using API.Models;

namespace API.Services
{
    public class PageService : IPageService
    {
        private readonly IPageRepository _pageRepository;

        public PageService(IPageRepository pageRepository)
        {
            _pageRepository = pageRepository;
        }

        public async Task<List<Page>> GetByContent(string? q, string? lang)
        {
            return await _pageRepository.GetByContent(q, lang);
        }
    }
}

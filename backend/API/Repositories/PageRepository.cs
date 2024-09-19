using API.Data;
using API.Interfaces;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class PageRepository : IPageRepository
    {
        private readonly DataContext _context;

        public PageRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<List<Page>> GetByContent(string? q, string? lang)
        {
            if (string.IsNullOrEmpty(q))
            {
                return new List<Page>();
            }
            return await _context.Pages.Where(p => p.Content.Contains(q) && p.Language.Equals(lang)).ToListAsync();
        }
    }
}

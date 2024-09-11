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

        public async Task<Page> GetByTitleAsync(string title)
        {
            return await _context.Pages.FirstOrDefaultAsync(p => p.Title == title);
        }
    }
}

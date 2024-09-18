using API.Interfaces;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("/api/search")]
    public class SearchController : Controller
    {
        private readonly ILogger<SearchController> _logger;
        private readonly IPageRepository _pageRepository;

        public SearchController(ILogger<SearchController> logger, IPageRepository pageRepository)
        {
            _logger = logger;
            _pageRepository = pageRepository;
        }

        [HttpGet(Name = "GetSearch")]
        public async Task<ActionResult<IEnumerable<Page>>> GetSearchResults([FromQuery] string q, [FromQuery] string? language = "en")
        {
            // Get search results
            var results = await _pageRepository.GetByContent(q, language);

            return Ok(results);
        }
    }
}

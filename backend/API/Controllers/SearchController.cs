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
        private readonly IPageService _pageService;

        public SearchController(ILogger<SearchController> logger, IPageService pageService)
        {
            _logger = logger;
            _pageService = pageService;
        }

        [HttpGet(Name = "GetSearch")]
        public async Task<ActionResult<IEnumerable<Page>>> GetSearchResults([FromQuery] string q, [FromQuery] string? language = "en")
        {
            // Get search results
            var results = await _pageService.GetByContent(q, language);

            return Ok(results);
        }
    }
}

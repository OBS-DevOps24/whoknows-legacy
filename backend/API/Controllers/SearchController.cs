using API.Interfaces;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("/api/search")]
    public class SearchController : ControllerBase
    {
        private readonly IPageService _pageService;

        public SearchController(IPageService pageService)
        {
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

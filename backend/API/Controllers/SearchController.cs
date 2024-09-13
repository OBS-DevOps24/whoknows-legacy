using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("/api/"+"[controller]")]
    public class SearchController : Controller
    {
        private readonly ILogger<SearchController> _logger;

        public SearchController(ILogger<SearchController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetSearch")]
        public IActionResult GetSearchResults([FromQuery] string q, [FromQuery] string language = "en")
        {
            if (string.IsNullOrEmpty(q))
            {
                return BadRequest("Query parameter 'q' is required.");
            }

            // Replace with your actual search logic
            var results = new List<string>
            {
                "Result for " + q
            };

            return Ok(results);
        }
    }
}

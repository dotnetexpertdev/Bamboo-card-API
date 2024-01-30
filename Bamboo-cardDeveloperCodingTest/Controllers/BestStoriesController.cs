using Bamboo_cardDeveloperCodingTest.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bamboo_cardDeveloperCodingTest.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class BestStoriesController : ControllerBase
	{
		private readonly IHackerNewsService _hackerNewsService;

		public BestStoriesController( IHackerNewsService hackerNewsService )
		{
			_hackerNewsService = hackerNewsService;
		}

		[HttpGet("{n}")]
		public async Task<IActionResult> GetBestStories( int n )
		{
			var bestStories = await _hackerNewsService.GetBestStoriesAsync(n);
			return Ok(bestStories);
		}
	}
}

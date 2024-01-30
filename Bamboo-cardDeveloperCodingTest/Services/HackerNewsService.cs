using Bamboo_cardDeveloperCodingTest.Models;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace Bamboo_cardDeveloperCodingTest.Services
{
	public interface IHackerNewsService
	{
		Task<List<HackerNewsStory>> GetBestStoriesAsync( int n );
	}
	public class HackerNewsService : IHackerNewsService
	{
		private readonly HttpClient _httpClient;

		private readonly IMemoryCache _cache;
		private readonly TimeSpan _cacheExpiration = TimeSpan.FromMinutes(5); // Adjust as needed


		public HackerNewsService( HttpClient httpClient, IMemoryCache cache)
		{
			_httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
			_cache = cache;
		}
		public async Task<List<HackerNewsStory>> GetBestStoriesAsync( int n )
		{
			if (_cache.TryGetValue("BestStories", out List<HackerNewsStory> cachedStories))
			{
				// Return cached data if available
				return cachedStories.Take(n).ToList();
			}
			// If not in cache, fetch data from Hacker News API
			var newStories = await FetchAndOrderBestStoriesAsync(n);

			// Cache the new data
			_cache.Set("BestStories", newStories, _cacheExpiration);
			
			return newStories;

		}

		private async Task<List<HackerNewsStory>> FetchAndOrderBestStoriesAsync( int n )
		{
			// Get the list of best story IDs
			var bestStoryIdsResponse = await _httpClient.GetStringAsync("https://hacker-news.firebaseio.com/v0/beststories.json");
			var bestStoryIds = JsonConvert.DeserializeObject<List<int>>(bestStoryIdsResponse);

			// Retrieve details for all the best stories
			var tasks = bestStoryIds.Select(async storyId =>
			{
				var storyDetailsResponse = await _httpClient.GetStringAsync($"https://hacker-news.firebaseio.com/v0/item/{storyId}.json");
				return JsonConvert.DeserializeObject<HackerNewsStory>(storyDetailsResponse);
			});

			var allBestStories = await Task.WhenAll(tasks);

			// Order by score in descending order and take the top N
			var topNStories = allBestStories.OrderByDescending(s => s.Score).Take(n).ToList();

			return topNStories;
		}
	}


}

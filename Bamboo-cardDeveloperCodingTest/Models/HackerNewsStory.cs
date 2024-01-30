using Newtonsoft.Json;

namespace Bamboo_cardDeveloperCodingTest.Models
{
	public class HackerNewsStory
	{

		[JsonProperty("By")]
		public string postedBy { get; set; }		
		public int Score { get; set; }
		public int Time { get; set; }
		public string Title { get; set; }
		public string Type { get; set; }
		
		[JsonProperty("Url")]
		public string uri { get; set; }
        public int commentCount { get; set; }

    }
}

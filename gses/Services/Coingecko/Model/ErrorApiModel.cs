using System.Text.Json.Serialization;

namespace Gses.Services.Coingecko.Model
{
	public class ErrorApiModel
	{
		[JsonPropertyName("error")] 
		public string? ErrorMessage { get; set; }
	}
}

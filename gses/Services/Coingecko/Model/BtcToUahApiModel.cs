using System.Text.Json.Serialization;

namespace Gses.Services.Coingecko.Model
{
	public class BtcToUahApiModel
	{
		[JsonPropertyName("bitcoin")] 
		public BtcToUahBitcoinApiModel? Bitcoin { get; set; }
	}

	public class BtcToUahBitcoinApiModel
	{
		[JsonPropertyName("uah")] 
		public decimal? Uah { get; set; }
	}
}

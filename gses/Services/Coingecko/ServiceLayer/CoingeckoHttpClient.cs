using System.Text.Json;
using Gses.Services.Coingecko.Model;
using Gses.Services.Common.Model;

namespace Gses.Services.Coingecko.ServiceLayer
{
	public interface ICoingeckoHttpClient
	{
		Task<HttpResponseEntityModel<BtcToUahApiModel>> GetBtcToUahActualRate();
	}

	public class CoingeckoHttpClient : ICoingeckoHttpClient
	{
		private readonly HttpClient _httpClient;

		public CoingeckoHttpClient(HttpClient httpClient, IConfiguration configuration)
		{
			_httpClient = httpClient;
			_httpClient.BaseAddress = new Uri(configuration["Coingecko:ApiUrl"]);
			_httpClient.Timeout = TimeSpan.FromSeconds(20);
		}

		public async Task<HttpResponseEntityModel<BtcToUahApiModel>> GetBtcToUahActualRate()
		{
			var response = await _httpClient.GetAsync("simple/price?ids=bitcoin&vs_currencies=uah");
			return await getResponseAsync<BtcToUahApiModel>(response);
		}

		private async Task<HttpResponseEntityModel<TEntity>> getResponseAsync<TEntity>(HttpResponseMessage response)
			where TEntity : class
		{
			var result = new HttpResponseEntityModel<TEntity> {
				Success = response.IsSuccessStatusCode
			};

			var responseContent = await response.Content.ReadAsStringAsync();
			if (!response.IsSuccessStatusCode)
			{
				var errorResponse = JsonSerializer.Deserialize<ErrorApiModel>(responseContent);
				result.Message = !string.IsNullOrEmpty(errorResponse?.ErrorMessage) ? errorResponse.ErrorMessage : responseContent;
				return result;
			}

			result.Entity = JsonSerializer.Deserialize<TEntity>(responseContent);
			return result;
		}
	}
}

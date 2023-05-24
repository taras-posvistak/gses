using Gses.Services.Coingecko.ServiceLayer;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Gses.Services.Rate.ServiceLayer
{
	public interface IRateService
	{
		Task<decimal?> GetBtcToUahActualRateAsync(ModelStateDictionary modelState);
	}

	public class RateService : IRateService
	{
		private readonly ICoingeckoHttpClient _coingeckoHttpClient;

		public RateService(ICoingeckoHttpClient coingeckoHttpClient)
		{
			_coingeckoHttpClient = coingeckoHttpClient;
		}

		public async Task<decimal?> GetBtcToUahActualRateAsync(ModelStateDictionary modelState)
		{
			var response = await _coingeckoHttpClient.GetBtcToUahActualRate();
			if (!response.Success && !string.IsNullOrEmpty(response.Message))
			{
				modelState.AddModelError("Error", response.Message);
				return null;
			}

			if (response.Entity?.Bitcoin?.Uah == null)
			{
				modelState.AddModelError("Error", "Failed to get bitcoin rate from third party service");
				return null;
			}

			return response.Entity?.Bitcoin?.Uah;
		}
	}
}

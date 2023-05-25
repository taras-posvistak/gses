using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using Gses.Services.Rate.ServiceLayer;

namespace Gses.Controllers
{
	[ApiController]
	[Route("rate")]
	[Tags("rate")]
	[Produces(MediaTypeNames.Application.Json)]
	public class RateController : Controller
	{
		private readonly IRateService _rateService;

		public RateController(IRateService rateService)
		{
			_rateService = rateService;
		}

		/// <summary>
		/// Отримати поточний курс BTC до UAH
		/// </summary>
		/// <response code="200">Повертається актуальний курс BTC до UAH</response>
		/// <response code="400">Опис помилки</response>
		[HttpGet]
		[ProducesResponseType(typeof(decimal?), StatusCodes.Status200OK)]
		public async Task<IActionResult> Get()
		{
			var rate = await _rateService.GetBtcToUahActualRateAsync(ModelState);
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			return Ok(rate);
		}
	}
}
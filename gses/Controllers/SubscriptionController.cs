using Microsoft.AspNetCore.Mvc;
using Gses.Services.Subscription.ServiceLayer;

namespace Gses.Controllers
{
	[ApiController]
	[Tags("subscription")]
	public class SubscriptionController : Controller
	{
		private readonly ISubscriptionService _subscriptionService;

		public SubscriptionController(ISubscriptionService subscriptionService)
		{
			_subscriptionService = subscriptionService;
		}

		/// <summary>
		/// Підписати e-mail на отримання поточного курсу
		/// </summary>
		/// <response code="200">E-mail додано</response>
		/// <response code="409">E-mail вже є в базі даних</response>
		[HttpPost("subscribe")]
		[Consumes("application/x-www-form-urlencoded")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public IActionResult Subscribe([FromForm] string email)
		{
			var success = _subscriptionService.Subscribe(email, ModelState);
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			if (!success)
			{
				return Conflict();
			}

			return Ok();
		}

		/// <summary>
		/// Відправити e-mail з поточним курсом на всі підписані електронні пошти
		/// </summary>
		/// <response code="200">E-mail-и відправлено</response>
		[HttpPost("sendEmails")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<IActionResult> SendEmails()
		{
			await _subscriptionService.NotifySubscribersAsync(ModelState);
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			return Ok();
		}
	}
}

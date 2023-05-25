using Gses.Services.Mail.Model;

namespace Gses.Services.Subscription.Model
{
	public class BtcToUahRateMailTemplateModel : IMailTemplateModel
	{
		public string Subject { get; set; }

		public string Date { get; set; }

		public decimal Rate { get; set; }

		public BtcToUahRateMailTemplateModel(decimal rate)
		{
			Date = DateTime.Now.ToString("dd.MM.yyyy HH:mm");
			Subject = $"Курс BTC до UAH на {Date}";
			Rate = rate;
		}
	}
}

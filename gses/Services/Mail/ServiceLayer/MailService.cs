using System.Net.Mail;
using System.Net;
using Gses.Services.Mail.Model;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Gses.Services.Mail.ServiceLayer
{
	public interface IMailService
	{
		Task NotifyAsync(string subject, string body, IEnumerable<string> recipientEmails, ModelStateDictionary? modelState = null);
	}

	public class MailService : IMailService
	{
		private readonly IConfiguration _configuration;

		public MailService(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public async Task NotifyAsync(string subject, string body, IEnumerable<string> recipientEmails,
			ModelStateDictionary? modelState = null)
		{
			foreach (var recipientEmail in recipientEmails)
			{
				await notifyAsync(subject, body, recipientEmail, modelState);
			}
		}

		private async Task notifyAsync(string subject, string body, string recipientEmail, ModelStateDictionary? modelState = null)
		{
			var config = _configuration.GetSection("MailServer").Get<MailServerConfigModel>();

			using var smtpClient = new SmtpClient {
				Host = config.SmtpServer,
				Port = config.SmtpPort,
				UseDefaultCredentials = false,
				EnableSsl = true,
				Credentials = new NetworkCredential(config.SmtpUserAddress, config.SmtpUserPassword)
			};

			try
			{
				using var message = new MailMessage(config.SmtpUserAddress, recipientEmail, subject, body) {
					IsBodyHtml = true
				};
				await smtpClient.SendMailAsync(message).ConfigureAwait(false);
			}
			catch (Exception e)
			{
				var errorMessage =
					$"An error occurred when sending e-mail. Recipient e-mail: {recipientEmail}. Error description: {e.Message}";
				if (modelState != null)
				{
					modelState.AddModelError("Error", errorMessage);
				}
				else
				{
					Console.WriteLine(errorMessage);
				}
			}
		}
	}
}
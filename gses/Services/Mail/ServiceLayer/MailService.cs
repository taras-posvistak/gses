using System.Net.Mail;
using System.Net;
using Gses.Services.Mail.Model;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Gses.Services.Mail.ServiceLayer
{
	public interface IMailService
	{
		Task NotifyAsync(string templateName, IMailTemplateModel templateModel, IEnumerable<string> recipientEmails,
			ModelStateDictionary? modelState = null);
	}

	public class MailService : IMailService
	{
		private readonly IHtmlTemplateEngineerService _htmlTemplateEngineerService;
		private readonly IConfiguration _configuration;

		public MailService(
			IHtmlTemplateEngineerService htmlTemplateEngineerService,
			IConfiguration configuration)
		{
			_htmlTemplateEngineerService = htmlTemplateEngineerService;
			_configuration = configuration;
		}

		public async Task NotifyAsync(string templateName, IMailTemplateModel templateModel, IEnumerable<string> recipientEmails,
			ModelStateDictionary? modelState = null)
		{
			foreach (var recipientEmail in recipientEmails)
			{
				await notifyAsync(templateName, templateModel, recipientEmail, modelState);
			}
		}

		private async Task notifyAsync(string templateName, IMailTemplateModel templateModel, string recipientEmail,
			ModelStateDictionary? modelState = null)
		{
			var config = _configuration.GetSection("MailServer").Get<MailServerConfigModel>();
			var smtpUserAddress = _configuration["SMTP_USER_ADDRESS"];

			using var smtpClient = new SmtpClient {
				Host = config.SmtpServer,
				Port = config.SmtpPort,
				UseDefaultCredentials = false,
				EnableSsl = true,
				Credentials = new NetworkCredential(smtpUserAddress, _configuration["SMTP_USER_PASSWORD"])
			};

			var mailContent = await getMailContent(templateName, templateModel);
			using var message = new MailMessage(smtpUserAddress, recipientEmail, mailContent.Subject, mailContent.Body) {
				IsBodyHtml = true
			};

			try
			{
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

		private async Task<MailContentModel> getMailContent(string templateName, IMailTemplateModel templateModel)
		{
			return new MailContentModel {
				Subject = templateModel.Subject,
				Body = await _htmlTemplateEngineerService.RenderViewToStringAsync(templateName, templateModel)
			};
		}
	}
}
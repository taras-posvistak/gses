﻿namespace Gses.Services.Mail.Model
{
	public class MailServerConfigModel
	{
		public string? SmtpServer { get; set; }

		public int SmtpPort { get; set; }

		public string? SmtpUserAddress { get; set; }

		public string? SmtpUserPassword { get; set; }
	}
}
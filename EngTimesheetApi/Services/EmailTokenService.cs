using EngTimesheetApi.Data;
using EngTimesheetApi.Interfaces;
using EngTimesheetApi.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Mail;
using System.Threading.Tasks;

namespace EngTimesheetApi.Services
{
	public class EmailTokenService : TokenService, IEmailTokenService
	{
		private int _smtpPort;
		private string _smtpHost;
		private string _fromAddress;

		public EmailTokenService(IConfiguration configuration, TokenServiceType type, TimesheetContext context, ILogger<EmailTokenService> logger)
			: base(Convert.ToInt32(configuration["TokenTimeout"]), type, context, logger)
		{
			_smtpPort = Convert.ToInt32(configuration["SMTP:Port"]);
			_smtpHost = configuration["SMTP:Host"];
			_fromAddress = configuration["From"];
		}

		public async Task SendEmailAsync(int id, string toAddress)
		{
			string token = await NewTokenAsync(id, true);
			try
			{
				SmtpClient client = new SmtpClient();
				client.Port = _smtpPort;
				client.DeliveryMethod = SmtpDeliveryMethod.Network;
				client.UseDefaultCredentials = false;
				client.Host = _smtpHost;

				MailMessage mail = new MailMessage(_fromAddress, toAddress);
				mail.Subject = "Engineering Timesheet Password Reset";
				mail.Body = $"The token is \"{token}\"";

				client.Send(mail);
			}
			catch(SmtpException ex)
			{
				_logger.LogError("could not send email, {0}", ex);
				_logger.LogInformation("TEMPORARY: {0}", token);
			}
		}
	}
}

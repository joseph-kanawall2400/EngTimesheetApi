using EngTimesheet.Data;
using EngTimesheet.Shared.Interfaces;
using EngTimesheet.Shared.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Mail;
using System.Threading.Tasks;

namespace EngTimesheet.Services
{
	public class EmailTokenService : TokenService, IEmailTokenService
	{
		private int _smtpPort;
		private string _smtpHost;
		private string _fromAddress;
		private string _frontEndResetUri;

		public EmailTokenService(IConfiguration configuration, TokenServiceType type, TimesheetContext context, ILogger<EmailTokenService> logger)
			: base(Convert.ToInt32(configuration["TokenTimeout"]), type, context, logger)
		{
			_smtpPort = Convert.ToInt32(configuration["SMTP:Port"]);
			_smtpHost = configuration["SMTP:Host"];
			_fromAddress = configuration["From"];
			_frontEndResetUri = configuration["FrontEndResetUri"];
		}

		public async Task<string> SendEmailAsync(int id, string toAddress)
		{
			string token = await NewTokenAsync(id, true);
			return token;
			try
			{
				SmtpClient client = new SmtpClient();
				client.Port = _smtpPort;
				client.DeliveryMethod = SmtpDeliveryMethod.Network;
				client.UseDefaultCredentials = false;
				client.Host = _smtpHost;

				MailMessage mail = new MailMessage(_fromAddress, toAddress);
				mail.Subject = "Engineering Timesheet Password Reset";
				mail.Body = $"Please reset your password at {_frontEndResetUri + token}";

				client.Send(mail);
			}
			catch(Exception)
			{
				// remove the token before throwing the error up to whatever is calling this
				await RemoveTokenAsync(token);
				throw;
			}
		}
	}
}

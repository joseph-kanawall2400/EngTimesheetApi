using EngTimesheetApi.Data;
using EngTimesheetApi.Interfaces;
using EngTimesheetApi.Models;
using Microsoft.Extensions.Configuration;
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

		public EmailTokenService(IConfiguration configuration, TokenServiceType type, TimesheetContext context)
			: base(Convert.ToInt32(configuration["TokenTimeout"]), type, context)
		{
			_smtpPort = Convert.ToInt32(configuration["SMTP:Port"]);
			_smtpHost = configuration["SMTP:Host"];
			_fromAddress = configuration["From"];
		}

		public async Task SendEmailAsync(int id, string toAddress)
		{
			SmtpClient client = new SmtpClient();
			client.Port = _smtpPort;
			client.DeliveryMethod = SmtpDeliveryMethod.Network;
			client.UseDefaultCredentials = false;
			client.Host = _smtpHost;

			MailMessage mail = new MailMessage(_fromAddress, toAddress);
			mail.Subject = "Engineering Timesheet Password Reset";
			mail.Body = $"The token is \"{await NewTokenAsync(id)}\"";

			client.Send(mail);
		}
	}
}

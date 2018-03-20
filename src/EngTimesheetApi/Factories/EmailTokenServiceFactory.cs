using EngTimesheetApi.Data;
using EngTimesheetApi.Models;
using EngTimesheetApi.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace EngTimesheetApi.Factories
{
	public static class EmailTokenServiceFactory
	{
		internal static EmailTokenService CreateEmailTokenService(IConfiguration configuration, IServiceProvider provider)
			=> new EmailTokenService(
				configuration, 
				TokenServiceType.Email, 
				provider.GetService<TimesheetContext>(),
				provider.GetService<ILogger<EmailTokenService>>()
			);
	}
}

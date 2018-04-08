using EngTimesheet.Data;
using EngTimesheet.Services;
using EngTimesheet.Shared.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace EngTimesheet.Factories
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

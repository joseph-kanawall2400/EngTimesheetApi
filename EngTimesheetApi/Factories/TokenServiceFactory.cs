using EngTimesheet.Data;
using EngTimesheet.Services;
using EngTimesheet.Shared.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace EngTimesheet.Factories
{
	public static class TokenServiceFactory
	{
		public static TokenService CreateTokenService(IConfiguration configuration, IServiceProvider provider)
			=> new TokenService(
				Convert.ToInt32(configuration["LoginTokenTimeout"]),
				TokenServiceType.Default,
				provider.GetService<TimesheetContext>(),
				provider.GetService<ILogger<TokenService>>()
			);
	}
}

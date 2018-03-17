using EngTimesheetApi.Data;
using EngTimesheetApi.Models;
using EngTimesheetApi.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace EngTimesheetApi.Factories
{
	public static class TokenServiceFactory
	{
		public static TokenService CreateTokenService(IConfiguration configuration, IServiceProvider provider)
			=> new TokenService(Convert.ToInt32(configuration["LoginTokenTimeout"]), TokenServiceType.Default, provider.GetService<TimesheetContext>());
	}
}

using JiraApi;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EngTimesheet.Pages
{
	[Produces("application/json")]
	[Route("api/Jira")]
	public class JiraController : Controller
	{
		private IConfiguration _configuration;

		public JiraController(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public async Task<IActionResult> GetAsync([FromQuery]string username, [FromQuery]string password, [FromQuery]DateTime date)
		{

			return Ok(new Dictionary<string, int> {
				{ "Maintenance", 100000 },
				{ "Enhancement", 162000},
				{ "Research And Development", 50236 }
			});
			Jira jira = new Jira(username, password, _configuration["Jira:ApiUri"], _configuration["Jira:FieldId"]);
			if(!await jira.IsAuthenticatedAsync())
			{
				return BadRequest();
			}
			return Ok(await jira.GetWorklogAsync(date, username));
		}
	}
}
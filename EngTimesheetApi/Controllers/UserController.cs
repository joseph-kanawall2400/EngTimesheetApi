using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EngTimesheetApi.Data;
using EngTimesheetApi.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EngTimesheetApi.Controllers
{
	[Produces("application/json")]
	[Route("api/User")]
	public class UserController : Controller
	{
		private TimesheetContext _context;
		private ITokenService _tokenService;
		private ILogger _logger;

		public UserController(TimesheetContext context, ITokenService tokenService, ILogger<AccountController> logger)
		{
			_context = context;
			_tokenService = tokenService;
			_logger = logger;
		}

		[HttpGet]
		public IActionResult Get()
		{
			if(!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			return Ok(_context.Users.Select(x => x.FirstName));
		}
	}
}
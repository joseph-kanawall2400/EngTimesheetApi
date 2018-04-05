using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EngTimesheetApi.Data;
using EngTimesheetApi.Interfaces;
using EngTimesheetApi.Mappers;
using EngTimesheetApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EngTimesheetApi.Controllers
{
	[Produces("application/json")]
	[Route("api/users")]
	public class UsersController : Controller
	{
		private TimesheetContext _context;
		private ITokenService _tokenService;
		private ILogger _logger;

		public UsersController(TimesheetContext context, ITokenService tokenService, ILogger<AccountController> logger)
		{
			_context = context;
			_tokenService = tokenService;
			_logger = logger;
		}

		[HttpPost]
		[Route("{id:int}")]
		public async Task<IActionResult> PostAsync([FromRoute] int id, [FromBody]UserDTO model)
		{
			if(!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			if(ModelState.ErrorCount != 0)
			{
				return BadRequest(ModelState);
			}
			return Ok();
		}

		[HttpGet]
		[Route("{id:int}")]
		public async Task<IActionResult> GetAsync([FromRoute]int id)
		{
			if(!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			User user = await _context.Users.SingleOrDefaultAsync(x => x.Id == id);
			if(user == null)
			{
				ModelState.AddModelError("UserNotExist", "User does not exist");
			}

			if(ModelState.ErrorCount != 0)
			{
				return BadRequest(ModelState);
			}
			return Ok(UserMapper.MapToUserDTO(user));
		}

		[HttpGet]
		[Route("{email}")]
		public async Task<IActionResult> GetAsync([FromRoute]string email)
		{
			if(!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			User user = await _context.Users.SingleOrDefaultAsync(x => x.Email == email);
			if(user == null)
			{
				ModelState.AddModelError("UserNotExist", "User does not exist");
			}

			if(ModelState.ErrorCount != 0)
			{
				return BadRequest(ModelState);
			}
			return Ok(UserMapper.MapToUserDTO(user));
		}

		[HttpGet]
		public async Task<IActionResult> GetAsync()
		{
			if(!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			User user = await _context.Users.SingleOrDefaultAsync(x => x.Email == email);
			if(user == null)
			{
				ModelState.AddModelError("UserNotExist", "User does not exist");
			}

			if(ModelState.ErrorCount != 0)
			{
				return BadRequest(ModelState);
			}
			return Ok(UserMapper.MapToUserDTO(user));
		}
	}
}
using EngTimesheet.Data;
using EngTimesheet.Shared;
using EngTimesheet.Shared.Interfaces;
using EngTimesheet.Shared.Mappers;
using EngTimesheet.Shared.Models;
using EngTimesheet.Shared.Validators;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EngTimesheet.Controllers
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

		[HttpGet]
		public async Task<IActionResult> GetAsync([FromHeader]string authToken)
		{
			if(!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			if(!await _tokenService.HasTokenAsync(authToken))
			{
				return Unauthorized();
			}

			return Ok(_context.Users.Select(x => UserMapper.MapToUserDTO(x)));
		}

		[HttpPost]
		public async Task<IActionResult> PostAsync([FromHeader]string authToken, [FromBody]UserDTO model)
		{
			if(!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			if(!await _tokenService.HasTokenAsync(authToken))
			{
				return Unauthorized();
			}

			User user = UserMapper.Map(model);

			UserValidator validator = new UserValidator();
			ValidationResult result = await validator.ValidateAsync(user);
			if(result.IsValid)
			{
				_context.Users.Update(user);
				await _context.SaveChangesAsync();
			}
			else
			{
				foreach(ValidationFailure error in result.Errors)
				{
					ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
				}
			}

			if(ModelState.ErrorCount != 0)
			{
				return BadRequest(ModelState);
			}
			return Ok();
		}

		[HttpGet]
		[Route("{authToken}")]
		public async Task<IActionResult> GetAuthAsync([FromRoute]string authToken)
		{
			if(!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			int userId = await _tokenService.GetIdAsync(authToken, true);
			if(userId == 0)
			{
				return Unauthorized();
			}
			return Ok(UserMapper.MapToUserDTO(await _context.Users.SingleOrDefaultAsync(x => x.Id == userId)));
		}

		[HttpGet]
		[Route("{id:int}/times")]
		public async Task<IActionResult> GetIdTimesAsync([FromHeader]string authToken, [FromRoute]int id)
		{
			if(!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			if(!await _tokenService.HasTokenAsync(authToken))
			{
				return Unauthorized();
			}

			User user = await _context.Users.Include(x => x.Times).SingleOrDefaultAsync(x => x.Id == id);
			if(user == null)
			{
				ModelState.AddModelError("UserNotExist", "User does not exist");
			}


			if(ModelState.ErrorCount != 0)
			{
				return BadRequest(ModelState);
			}
			return Ok(TimeMapper.MapToTimeDTO(user));
		}

		[HttpPost]
		[Route("{id:int}/times")]
		public async Task<IActionResult> PostIdTimesAsync([FromHeader]string authToken, [FromRoute]int id, [FromBody]TimeDTO model)
		{
			if(!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			if(!await _tokenService.HasTokenAsync(authToken))
			{
				return Unauthorized();
			}

			User user = await _context.Users.Include(x => x.Times).SingleOrDefaultAsync(x => x.Id == id);
			if(user == null)
			{
				ModelState.AddModelError("UserNotExist", "User does not exist");
			}
			else
			{
				Time time = user.Times.SingleOrDefault(x => x.Category == model.Category && x.Date == model.Date.FirstOfMonth());

				if(time == null)
				{
					user.Times.Add(TimeMapper.Map(model));
				}
				else
				{
					int index = user.Times.IndexOf(time);
					user.Times[index] = TimeMapper.Map(model, time);
				}

				try
				{
					_context.Users.Update(user);
					await _context.SaveChangesAsync();
				}
				catch(Exception ex)
				{
					ModelState.AddModelError("Error", ex.Message);
				}
			}


			if(ModelState.ErrorCount != 0)
			{
				return BadRequest(ModelState);
			}
			return Ok();
		}

		[HttpGet]
		[Route("{id:int}/times/{date:datetime}")]
		public async Task<IActionResult> GetIdTimesAsync([FromHeader]string authToken, [FromRoute]int id, [FromRoute]DateTime date)
		{
			if(!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			if(!await _tokenService.HasTokenAsync(authToken))
			{
				return Unauthorized();
			}

			User user = await _context.Users.Include(x => x.Times).SingleOrDefaultAsync(x => x.Id == id);
			if(user == null)
			{
				ModelState.AddModelError("UserNotExist", "User does not exist");
			}


			if(ModelState.ErrorCount != 0)
			{
				return BadRequest(ModelState);
			}
			return Ok(TimeMapper.MapToTimeDTO(user).Where(x => x.Date == date.FirstOfMonth()));
		}

		private User GetUser(int id) => _context.Users.SingleOrDefault(x => x.Id == id);
	}
}
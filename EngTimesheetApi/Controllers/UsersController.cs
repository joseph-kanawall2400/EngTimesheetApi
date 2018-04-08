using EngTimesheetApi.Data;
using EngTimesheetApi.Shared.Interfaces;
using EngTimesheetApi.Shared.Mappers;
using EngTimesheetApi.Shared.Models;
using EngTimesheetApi.Shared.Validators;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

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
		[Route("{id:int}")]
		public async Task<IActionResult> GetIdAsync([FromHeader]string authToken, [FromRoute]int id)
		{
			if(!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			if(!await _tokenService.HasTokenAsync(authToken))
			{
				return Unauthorized();
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
	}
}
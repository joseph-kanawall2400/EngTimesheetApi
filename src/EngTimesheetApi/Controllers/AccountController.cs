using EngTimesheetApi.Data;
using EngTimesheetApi.Interfaces;
using EngTimesheetApi.Mappers;
using EngTimesheetApi.Models;
using EngTimesheetApi.Validators;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace EngTimesheetApi.Controllers
{
	[Produces("application/json")]
	[Route("api/Account")]
	public class AccountController : Controller
	{
		private TimesheetContext _context;
		private IEmailTokenService _emailTokenService;
		private ILogger _logger;

		public AccountController(TimesheetContext context, IEmailTokenService emailTokenService, ILogger<AccountController> logger)
		{
			_context = context;
			_emailTokenService = emailTokenService;
			_logger = logger;
		}

		[HttpPost]
		[Route("Register")]
		public async Task<IActionResult> RegisterAsync([FromBody]AccountRegisterDTO model)
		{
			if(!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			else
			{
				User user = UserMapper.Map(model);

				UserValidator validator = new UserValidator();
				ValidationResult result = await validator.ValidateAsync(user);
				if(result.IsValid)
				{
					if(!await _context.Users.AnyAsync(x => x.Email == user.Email))
					{
						_context.Users.Add(user);
						await _context.SaveChangesAsync();
						await _emailTokenService.SendEmailAsync(user.Id, user.Email);
					}
					else
					{
						ModelState.AddModelError("UserEmailExists", "A user with the email already exists");
					}
				}
				else
				{
					foreach(ValidationFailure error in result.Errors)
					{
						ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
					}
				}
			}

			if(ModelState.ErrorCount != 0)
			{
				return BadRequest(ModelState);
			}
			return Ok();
		}

		[HttpPost]
		[Route("Password")]
		public async Task<IActionResult> PasswordAsync([FromBody]AccountPasswordDTO model)
		{
			int userId = await _emailTokenService.GetIdAsync(model.Token);
			if(userId != 0)
			{
				Login login = (await _context.Logins.Include(x => x.User).SingleOrDefaultAsync(x => x.User.Id == userId)) ?? new Login();
				
				// The login is not created for the user by default, so now that registering is being completed,
				// create the new login and update the Registered field for the user
				if(login.User == null)
				{
					login.User = await _context.Users.SingleOrDefaultAsync(x => x.Id == userId);
					login.User.Registered = DateTime.Now;

					// If there was no user in the database with the id, then there is something wrong
					// because there is a token with the userId
					if(login.User == null)
					{
						_logger.LogError("A user was not present when there should be an Id, Id: {0}", userId);
						return StatusCode(500);
					}
				}

				login.SetPassword(model.Password);

				_context.Update(login);
				await _context.SaveChangesAsync();

			}
			else
			{
				ModelState.AddModelError("NoMatchingToken", "The provided token did not match");
			}

			if(ModelState.ErrorCount != 0)
			{
				return BadRequest(ModelState);
			}
			return Ok();
		}
	}
}
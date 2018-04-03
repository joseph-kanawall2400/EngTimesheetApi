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
using System.Linq;
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
		[Route("register")]
		public async Task<IActionResult> RegisterAsync([FromBody]AccountRegisterDTO model)
		{
			if(!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			User user = UserMapper.Map(model);

			UserValidator validator = new UserValidator();
			ValidationResult result = await validator.ValidateAsync(user);
			if(result.IsValid)
			{
				if(!await _context.Users.AnyAsync(x => x.Email == user.Email))
				{
					_context.Users.Add(user);
					await _context.SaveChangesAsync();
					try
					{
						await _emailTokenService.SendEmailAsync(user.Id, user.Email);
					}
					catch(Exception ex)
					{
						_logger.LogError("could not send email, {0}", ex);

						_context.Users.Remove(user);
						await _context.SaveChangesAsync();

						return StatusCode(500);
					}
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

			if(ModelState.ErrorCount != 0)
			{
				return BadRequest(ModelState);
			}
			return Ok();
		}

		[HttpPost]
		[Route("password/{email}")]
		public async Task<IActionResult> PasswordResetAsync([FromRoute]string email)
		{
			if(!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			User user = await _context.Users.SingleOrDefaultAsync(x => x.Email == email);
			if(user == null)
			{
				ModelState.AddModelError("UserNotExist", "A user with the email does not exist");
			}
			else
			{
				await _emailTokenService.SendEmailAsync(user.Id, user.Email);
			}

			if(ModelState.ErrorCount != 0)
			{
				return BadRequest(ModelState);
			}
			return Ok();
		}

		[HttpPost]
		[Route("password")]
		public async Task<IActionResult> PasswordAsync([FromBody]AccountPasswordDTO model)
		{
			if(!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			int userId = await _emailTokenService.GetIdAsync(model.Token);
			if(userId != 0)
			{
				Login login = (await _context.Logins.Include(x => x.User).SingleOrDefaultAsync(x => x.User.Id == userId)) ?? new Login();

				// The login is not created for the user by default, so now that registering is being completed,
				// create the new login and update the Registered field for the user
				if(login.User == null)
				{
					login.User = await _context.Users.SingleOrDefaultAsync(x => x.Id == userId);

					// If there was no user in the database with the id, then there is something wrong
					// because there is a token with the userId
					if(login.User == null)
					{
						_logger.LogError("A user was not present when there should be an Id, Id: {0}", userId);
						return StatusCode(500);
					}

					login.User.Registered = DateTime.Now;
				}

				login.SetPassword(model.Password);

				// Login must be updated because a new one could be created
				_context.Update(login);
				await _context.SaveChangesAsync();

			}
			else
			{
				ModelState.AddModelError("NoValidToken", "The provided token is not valid");
			}


			if(ModelState.ErrorCount != 0)
			{
				return BadRequest(ModelState);
			}
			return Ok();
		}

		[HttpGet]
		[Route("login")]
		public async Task<IActionResult> LoginAsync([FromQuery]string username, [FromQuery]string password)
		{
			if(!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			if(String.IsNullOrWhiteSpace(username))
			{
				ModelState.AddModelError("EmptyUsername", "Username is empty");
			}

			if(String.IsNullOrWhiteSpace(password))
			{
				ModelState.AddModelError("EmptyPassword", "Password is empty");
			}

			if(ModelState.ErrorCount == 0)
			{
				Login login = await _context.Logins.Include(x => x.User).SingleOrDefaultAsync(x => x.User.Email == username);
				if(login == null || !(login?.CheckPassword(password) ?? false))
				{
					ModelState.AddModelError("CredentialsInvalid", "Username and password do not match any registered users");
				}
				else
				{
					return Ok(new { Token = await _emailTokenService.NewTokenAsync(login.User.Id, false) });
				}
			}

			if(ModelState.ErrorCount != 0)
			{
				return BadRequest(ModelState);
			}
			// If execution has reached this point, then the Ok path was not reached, but there was no
			// error added to the ModelState
			_logger.LogError("The execution should not reach this point");
			return StatusCode(500);
		}
	}
}
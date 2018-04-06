using EngTimesheetApi.Data;
using EngTimesheetApi.Shared.Interfaces;
using EngTimesheetApi.Shared.Mappers;
using EngTimesheetApi.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

			_context.Users.Update(UserMapper.Map(model));
			await _context.SaveChangesAsync();

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
		public IActionResult GetAsync()
		{
			if(!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			return Ok(_context.Users.Select(x => UserMapper.MapToUserDTO(x)));
		}
	}
}
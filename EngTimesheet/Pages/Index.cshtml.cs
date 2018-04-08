using EngTimesheet.Services;
using EngTimesheetApi.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace EngTimesheet.Pages
{
	public class IndexModel : PageModel
	{
		WebApiService _webApi;

		[BindProperty]
		public string Message { get; set; }
		[EmailAddress]
		[BindProperty]
		public string Email { get; set; }
		[BindProperty]
		public string Password { get; set; }

		public IndexModel(WebApiService webApi)
		{
			_webApi = webApi;
		}

		public IActionResult OnGet()
		{
			if(!String.IsNullOrWhiteSpace(HttpContext.Session.GetString("token")))
			{
				return RedirectToPage("/Time");
			}
			return Page();
		}

		public async Task<IActionResult> OnPostAsync()
		{
			string token = await _webApi.Login(Email, Password);
			if(!String.IsNullOrWhiteSpace(token))
			{
				UserDTO user = await _webApi.GetUser(token);
				HttpContext.Session.SetString("token", token);
				return RedirectToPage("/Time");
			}
			Message = "Invalid Login";
			return Page();
		}
	}
}
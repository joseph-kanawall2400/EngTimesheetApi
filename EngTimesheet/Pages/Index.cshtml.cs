using EngTimesheet.Services;
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

		public void OnGet() { }

		public async Task<IActionResult> OnPostAsync()
		{
			string token = await _webApi.Login(Email, Password);
			if(!String.IsNullOrWhiteSpace(token))
			{
				HttpContext.Session.SetString("token", token);
				return RedirectToPage("/time");
			}
			Message = "Invalid Login";
			return Page();
		}
	}
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using EngTimesheet.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EngTimesheet.Pages
{
    public class RegisterModel : PageModel
    {
		WebApiService _webApi;

		[BindProperty]
		public string Message { get; set; }
		[BindProperty]
		[EmailAddress]
		public string Email { get; set; }
		[BindProperty]
		public string FirstName { get; set; }
		[BindProperty]
		public string LastName { get; set; }

		public RegisterModel(WebApiService webApi)
		{
			_webApi = webApi;
		}

		public void OnGet() { }

		public async Task<IActionResult> OnPostAsync()
		{
			try
			{
				string token = await _webApi.Register(Email, FirstName, LastName);
				Message = $"To set password go to http://engtimesheet.azurewebsites.net/reset/{token.Substring(1, token.Length - 2)}";
				//return RedirectToPage("/index");
			}
			catch(Exception ex)
			{
				Message = ex.Message;
			}
			return Page();
		}
	}
}
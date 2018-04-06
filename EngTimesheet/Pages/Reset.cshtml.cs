using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using EngTimesheet.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EngTimesheet.Pages
{
	public class ResetModel : PageModel
	{
		WebApiService _webApi;

		[BindProperty]
		public string Message { get; set; }
		[BindProperty]
		public string Token { get; set; }

		[BindProperty]
		public string Email { get; set; }

		[BindProperty]
		public string Password { get; set; }
		[BindProperty]
		[DisplayName("Confirm Password")]
		public string PasswordCheck { get; set; }

		public ResetModel(WebApiService webApi)
		{
			_webApi = webApi;
		}

		public void OnGet(string token)
		{
			Token = token;
		}

		public Task<IActionResult> OnPostAsync(string token)
		{
			return String.IsNullOrWhiteSpace(token) ? SendEmail() : ResetPassword(token);
		}

		private async Task<IActionResult> ResetPassword(string token)
		{
			if(Password == PasswordCheck)
			{
				try
				{
					await _webApi.ResetPassword(token, Password);
					Message = "Password was reset";
				}
				catch(Exception ex)
				{
					Message = ex.Message;
				}
			}
			else
			{
				Message = "Passwords do not match";
			}
			return Page();
		}

		private async Task<IActionResult> SendEmail()
		{
			try
			{
				await _webApi.SendPasswordEmail(Email);
				Message = "Email was sent";
			}
			catch(Exception ex)
			{
				Message = ex.Message;
			}
			return Page();
		}
	}
}
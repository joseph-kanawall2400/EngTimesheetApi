using EngTimesheet.Services;
using EngTimesheet.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Threading.Tasks;

namespace EngTimesheet.Pages
{
	public class SettingsModel : PageModel
	{
		private WebApiService _webApi;

		[BindProperty]
		public string Message { get; set; }
		[BindProperty]
		public UserDTO Model { get; set; }

		public SettingsModel(WebApiService webApi)
		{
			_webApi = webApi;
		}

		public async Task<IActionResult> OnGetAsync()
		{
			string token = HttpContext.Session.GetString("token");
			if(token == null)
			{
				return RedirectToPage("/Index");
			}

			try
			{
				Model = await _webApi.GetUser(token);
			}
			catch(Exception ex)
			{
				Message = ex.Message;
			}

			return Page();
		}

		public async Task<IActionResult> OnPostAsync()
		{
			string token = HttpContext.Session.GetString("token");
			if(token == null)
			{
				return RedirectToPage("/Index");
			}

			try
			{
				await _webApi.UpdateUser(token, Model);
				Message = "Your settings were updated";
			}
			catch(Exception ex)
			{
				Message = ex.Message;
			}

			return Page();
		}
	}
}
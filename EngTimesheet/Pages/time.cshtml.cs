using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EngTimesheet.Services;
using EngTimesheetApi.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EngTimesheet.Pages
{
    public class TimeModel : PageModel
    {

		private WebApiService _webApi;

		[BindProperty]
		public UserDTO Model { get; set; }

		public TimeModel(WebApiService webApi)
		{
			_webApi = webApi;
		}

		public IActionResult OnGet()
		{
			string token = HttpContext.Session.GetString("token");
			if(token == null)
			{
				return RedirectToPage("/Index");
			}


			return Page();
		}
	}
}
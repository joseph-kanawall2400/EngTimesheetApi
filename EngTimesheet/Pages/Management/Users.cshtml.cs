using EngTimesheet.Services;
using EngTimesheet.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EngTimesheet.Pages.Management
{
	public class UsersModel : PageModel
	{
		private WebApiService _webApi;

		[BindProperty]
		public string Message { get; set; }
		[BindProperty]
		public UserDTO UserModel { get; set; }
		[BindProperty]
		public List<UserDTO> Users { get; set; }

		public UsersModel(WebApiService webApi)
		{
			_webApi = webApi;
		}

		public async Task<IActionResult> OnGet()
		{
			string token = HttpContext.Session.GetString("token");
			if(token == null)
			{
				return RedirectToPage("/Index");
			}

			UserModel = await _webApi.GetUser(token);
			if(UserModel == null || !UserModel.Manager)
			{
				return RedirectToPage("/Unauthorized");
			}

			try
			{
				Users = await _webApi.GetUsers(token);
				
			}
			catch(Exception ex)
			{
				Message = ex.Message;
			}

			return Page();
		}

		public async Task<IActionResult> OnPostAsync(DateTime? date)
		{
			string token = HttpContext.Session.GetString("token");
			if(token == null)
			{
				return RedirectToPage("/Index");
			}

			UserModel = await _webApi.GetUser(token);
			if(UserModel == null || !UserModel.Manager)
			{
				return RedirectToPage("/Unauthorized");
			}

			try
			{
				foreach(UserDTO user in Users)
				{
					await _webApi.UpdateUser(token, user);
				}
				Message = "Users updated";
			}
			catch(Exception ex)
			{
				Message = ex.Message;
			}

			return Page();
		}
	}
}
using EngTimesheet.Services;
using EngTimesheet.Shared;
using EngTimesheet.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EngTimesheet.Pages.Management
{
	public class TimeModel : PageModel
	{
		private WebApiService _webApi;

		[BindProperty]
		public string Message { get; set; }
		[BindProperty]
		public DateTime Date { get; set; }
		[BindProperty]
		public UserDTO UserModel { get; set; }
		[BindProperty]
		public List<UserTimeDTO> Users { get; set; } = new List<UserTimeDTO>();

		public TimeModel(WebApiService webApi)
		{
			_webApi = webApi;
		}

		public async Task<IActionResult> OnGet(DateTime? date)
		{
			DateTime now = DateTime.Now.FirstOfMonth();

			string token = HttpContext.Session.GetString("token");
			if(token == null)
			{
				return RedirectToPage("/Index");
			}

			Date = date?.FirstOfMonth() ?? now;
			if(Date > now)
			{
				return Redirect("/Management/Time");
			}

			UserModel = await _webApi.GetUser(token);
			if(UserModel == null || !UserModel.Manager)
			{
				return RedirectToPage("/Unauthorized");
			}

			try
			{
				Users.Clear();
				foreach(UserDTO user in (await _webApi.GetUsers(token)).Where(x => x.Deactivated == null || x.Deactivated > Date))
				{
					Users.Add(new UserTimeDTO
					{
						User = user,
						Times = await _webApi.GetTimes(token, user.Id, Date)
					});
				}
				FillUserTimes();
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

			Date = date?.FirstOfMonth() ?? DateTime.Now.FirstOfMonth();

			UserModel = await _webApi.GetUser(token);
			if(UserModel == null || !UserModel.Manager)
			{
				return RedirectToPage("/Unauthorized");
			}

			try
			{
				foreach(UserTimeDTO user in Users)
				{
					foreach(TimeDTO time in user.Times)
					{
						await _webApi.UpdateTime(token, user.User.Id, time);
					}
				}
				Message = "User times updated";
			}
			catch(Exception ex)
			{
				Message = ex.Message;
			}

			return Page();
		}

		private void FillUserTimes()
		{
			for(int i = 0; i < Users.Count; i++)
			{
				foreach(TimeCategory cat in Enum.GetValues(typeof(TimeCategory)))
				{
					bool hasCat = false;
					foreach(TimeDTO time in Users[i].Times)
					{
						if(time.Category == cat)
						{
							hasCat = true;
							break;
						}
					}

					if(!hasCat)
					{
						Users[i].Times.Add(new TimeDTO { Category = cat, Date = Date });
					}
				}
			}
		}
	}
}
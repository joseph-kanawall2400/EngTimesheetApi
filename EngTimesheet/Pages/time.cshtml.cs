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

namespace EngTimesheet.Pages
{
	public class TimeModel : PageModel
	{

		private WebApiService _webApi;

		[BindProperty]
		public UserDTO UserModel { get; set; }
		[BindProperty]
		public DateTime Date { get; set; }
		[BindProperty]
		public string Message { get; set; }

		// The times are decimals instead of doubles because equal comaprison does not work
		[BindProperty]
		public decimal Maintenance { get; set; }
		[BindProperty]
		public decimal MaintenanceOriginal { get; set; }

		[BindProperty]
		public decimal Enhancement { get; set; }
		[BindProperty]
		public decimal EnhancementOriginal { get; set; }

		[BindProperty]
		public decimal NewDevelopment { get; set; }
		[BindProperty]
		public decimal NewDevelopmentOriginal { get; set; }

		[BindProperty]
		public decimal ResearchAndDevelopment { get; set; }
		[BindProperty]
		public decimal ResearchAndDevelopmentOriginal { get; set; }

		[BindProperty]
		public decimal Management { get; set; }
		[BindProperty]
		public decimal ManagementOriginal { get; set; }


		public TimeModel(WebApiService webApi)
		{
			_webApi = webApi;
		}

		public async Task<IActionResult> OnGetAsync(DateTime? date)
		{
			string token = HttpContext.Session.GetString("token");
			//if(token == null)
			//{
			//	return RedirectToPage("/Index");
			//}
			token = "a8c85eb2-d085-4471-9a3e-8a2c09c92638";

			Date = date?.FirstOfMonth() ?? DateTime.Now.FirstOfMonth();
			if(Date > DateTime.Now.FirstOfMonth())
			{
				return Redirect("/Time");
			}

			try
			{
				UserModel = await _webApi.GetUser(token);
				foreach(TimeDTO time in await _webApi.GetTimes(token, UserModel.Id, Date))
				{
					switch(time.Category)
					{
						case TimeCategory.Maintenance:
							Maintenance = MaintenanceOriginal = (decimal)time.Amount;
							break;
						case TimeCategory.Enhancement:
							Enhancement = EnhancementOriginal = (decimal)time.Amount;
							break;
						case TimeCategory.NewDevelopment:
							NewDevelopment = NewDevelopmentOriginal = (decimal)time.Amount;
							break;
						case TimeCategory.ResearchAndDevelopment:
							ResearchAndDevelopment = ResearchAndDevelopmentOriginal = (decimal)time.Amount;
							break;
						case TimeCategory.Management:
							Management = ManagementOriginal = (decimal)time.Amount;
							break;
					}
				}
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
			//if(token == null)
			//{
			//	return RedirectToPage("/Index");
			//}
			token = "a8c85eb2-d085-4471-9a3e-8a2c09c92638";

			Date = date?.FirstOfMonth() ?? DateTime.Now.FirstOfMonth();

			UserModel = await _webApi.GetUser(token);


			try
			{
				await UpdateTime(token, UserModel.Id, Maintenance, MaintenanceOriginal, TimeCategory.Maintenance);
				await UpdateTime(token, UserModel.Id, Enhancement, EnhancementOriginal, TimeCategory.Enhancement);
				await UpdateTime(token, UserModel.Id, NewDevelopment, NewDevelopmentOriginal, TimeCategory.NewDevelopment);
				await UpdateTime(token, UserModel.Id, ResearchAndDevelopment, ResearchAndDevelopmentOriginal, TimeCategory.ResearchAndDevelopment);
				await UpdateTime(token, UserModel.Id, Management, ManagementOriginal, TimeCategory.Management);
			}
			catch(Exception ex)
			{
				Message = ex.Message;
			}

			Message = "TEST";
			return Redirect("/Time");
		}

		private Task UpdateTime(string token, int id, decimal amount, decimal originalAmount, TimeCategory category)
		{
			if(amount != originalAmount)
			{
				return _webApi.UpdateTime(token, id, new TimeDTO
				{
					Amount = (double)amount,
					Category = category,
					Date = Date
				});
			}
			return Task.CompletedTask;
		}
	}
}
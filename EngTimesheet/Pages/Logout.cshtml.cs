using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EngTimesheet.Pages
{
	public class LogoutModel : PageModel
	{
		public void OnGet() => HttpContext.Session.Remove("token");
	}
}
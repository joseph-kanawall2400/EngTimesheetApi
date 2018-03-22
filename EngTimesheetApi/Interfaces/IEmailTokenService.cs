using System.Threading.Tasks;

namespace EngTimesheetApi.Interfaces
{
	public interface IEmailTokenService : ITokenService
	{
		Task SendEmailAsync(int id, string address);
	}
}

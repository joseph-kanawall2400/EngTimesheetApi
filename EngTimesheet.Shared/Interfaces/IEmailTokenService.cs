using System.Threading.Tasks;

namespace EngTimesheetApi.Shared.Interfaces
{
	public interface IEmailTokenService : ITokenService
	{
		Task SendEmailAsync(int id, string address);
	}
}

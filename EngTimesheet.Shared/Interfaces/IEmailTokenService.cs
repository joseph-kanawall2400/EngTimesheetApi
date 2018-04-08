using System.Threading.Tasks;

namespace EngTimesheet.Shared.Interfaces
{
	public interface IEmailTokenService : ITokenService
	{
		Task SendEmailAsync(int id, string address);
	}
}

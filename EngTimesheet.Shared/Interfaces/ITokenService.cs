using System.Threading.Tasks;

namespace EngTimesheetApi.Shared.Interfaces
{
	public interface ITokenService
	{
		Task<string> NewTokenAsync(int id, bool singleUse);
		Task<bool> HasTokenAsync(string token);
		Task<int> GetIdAsync(string token);
		Task<int> GetIdAsync(string token, bool refresh);
	}
}

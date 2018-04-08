using EngTimesheetApi.Shared.Models;
using System.Threading.Tasks;

namespace EngTimesheetApi.Shared.Interfaces
{
	public interface ITokenService
	{
		Task<string> NewTokenAsync(int id, bool singleUse);
		Task<string> NewTokenAsync(int id, bool singleUse, TokenServiceType? type);
		Task<bool> HasTokenAsync(string token);
		Task<int> GetIdAsync(string token);
		Task<int> GetIdAsync(string token, TokenServiceType? type);
		Task<int> GetIdAsync(string token, bool refresh);
		Task<int> GetIdAsync(string token, bool refresh, TokenServiceType? type);
		Task<string> GetTokenAsync(int id);
		Task<string> GetTokenAsync(int id, TokenServiceType? type);
	}
}

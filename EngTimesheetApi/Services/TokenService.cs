using EngTimesheet.Data;
using EngTimesheet.Shared.Interfaces;
using EngTimesheet.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EngTimesheet.Services
{
	public class TokenService : ITokenService
	{
		protected int _timeoutSeconds;
		protected TokenServiceType _type;
		protected TimesheetContext _context;
		protected ILogger<TokenService> _logger;

		public TokenService(int timeout, TokenServiceType type, TimesheetContext context, ILogger<TokenService> logger)
		{
			_timeoutSeconds = timeout;
			_type = type;
			_context = context;
			_logger = logger;
		}

		public Task<string> NewTokenAsync(int id, bool singleUse) => NewTokenAsync(id, singleUse, null);

		public async Task<string> NewTokenAsync(int id, bool singleUse, TokenServiceType? type)
		{
			Token token = await _context.Tokens.Include(x => x.User).SingleOrDefaultAsync(x => x.User.Id == id);

			if(token == null)
			{
				token = new Token
				{
					User = await _context.Users.SingleAsync(x => x.Id == id),
					Type = type ?? _type,
					SingleUse = true
				};
			}

			token.Value = Guid.NewGuid().ToString();
			token.Expired = DateTime.Now.AddSeconds(_timeoutSeconds);

			_context.Update(token);
			await _context.SaveChangesAsync();

			return token.Value;
		}

		public Task<int> GetIdAsync(string token, bool refresh) => GetIdAsync(token, refresh, null);

		public async Task<int> GetIdAsync(string token, bool refresh, TokenServiceType? type)
		{
			await ClearTokensAsync();
			// If the token is not there, then the id returned will be 0
			Token tokenItem = await _context.Tokens.Include(x => x.User).SingleOrDefaultAsync(x => x.Type == (type ?? _type) && x.Value == token);
			if(tokenItem != null)
			{
				if(refresh)
				{
					tokenItem.Expired = NewExpired();
				} else if(tokenItem.SingleUse)
				{
					_context.Tokens.Remove(tokenItem);
				}
				await _context.SaveChangesAsync();

			}
			return tokenItem?.User.Id ?? 0;
		}

		public Task<int> GetIdAsync(string token) => GetIdAsync(token, null);

		public Task<int> GetIdAsync(string token, TokenServiceType? type)
		{
			return GetIdAsync(token, false, type);
		}

		public Task<string> GetTokenAsync(int id) => GetTokenAsync(id, null);

		public async Task<string> GetTokenAsync(int id, TokenServiceType? type) => (await _context.Tokens.Include(x => x.User).SingleOrDefaultAsync(x => x.Type == (type ?? _type) && x.User.Id == id)).Value;

		public Task<bool> HasTokenAsync(string token)
		{
			return _context.Tokens.AnyAsync(x => x.Value == token);
		}

		protected async Task RemoveTokenAsync(string token)
		{
			Token tokenItem = await _context.Tokens.SingleOrDefaultAsync(x => x.Value == token);
			if(tokenItem != null)
			{
				_context.Tokens.Remove(tokenItem);
				await _context.SaveChangesAsync();
			}
		}

		private async Task ClearTokensAsync()
		{
			_context.Tokens.RemoveRange(_context.Tokens.Where(x => x.Expired < DateTime.Now));
			await _context.SaveChangesAsync();
		}

		private DateTime NewExpired() => DateTime.Now.AddSeconds(_timeoutSeconds);
	}
}

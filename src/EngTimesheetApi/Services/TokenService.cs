﻿using EngTimesheetApi.Data;
using EngTimesheetApi.Interfaces;
using EngTimesheetApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EngTimesheetApi.Services
{
	public class TokenService : ITokenService
	{
		protected int _timeoutSeconds;
		protected TokenServiceType _type;
		protected TimesheetContext _context;

		/// <summary>
		/// Initializes a new instance of TokenService with a specified timeout
		/// </summary>
		/// <param name="timeoutSeconds">An integer of how many seconds into the future the expiration of a token will be set</param>
		public TokenService(int timeout, TokenServiceType type, TimesheetContext context)
		{
			_timeoutSeconds = timeout;
			_type = type;
			_context = context;
		}

		/// <summary>
		/// Generates a new token for the given id with a new expiration date.
		/// If there was already a token for the id, it is overwritten.
		/// </summary>
		/// <param name="id">An integer of the id to generate a token for</param>
		/// <returns></returns>
		public async Task<string> NewTokenAsync(int id)
		{
			Token token = await _context.Tokens.Include(x => x.User).SingleOrDefaultAsync(x => x.User.Id == id);

			if(token == null)
			{
				token = new Token
				{
					User = await _context.Users.SingleAsync(x => x.Id == id),
					Type = _type
				};
			}

			token.Value = Guid.NewGuid().ToString();
			token.Expired = DateTime.Now.AddSeconds(_timeoutSeconds);

			_context.Update(token);
			await _context.SaveChangesAsync();

			return token.Value;
		}

		/// <summary>
		/// Returns the id matched with the token. If there is a match 
		/// and refresh is true, the expiration date is refreshed.
		/// </summary>
		/// <param name="token">A string of the token generated for the id</param>
		/// <param name="refresh">If true, the token expiration is reset</param>
		/// <returns></returns>
		public async Task<int> GetIdAsync(string token, bool refresh)
		{
			await ClearTokensAsync();
			// If the token is not there, then the id returned will be 0
			Token tokenItem = await _context.Tokens.Include(x => x.User).SingleOrDefaultAsync(x => x.Type == _type && x.Value == token);
			if(refresh && tokenItem != null)
			{
				tokenItem.Expired = NewExpired();
				await _context.SaveChangesAsync();
			}
			return tokenItem?.User.Id ?? 0;
		}

		/// <summary>
		/// Returns the id matched with the token.
		/// </summary>
		/// <param name="token">A string of the token generated for the id</param>
		/// <returns></returns>
		public Task<int> GetIdAsync(string token)
		{
			return GetIdAsync(token, false);
		}

		/// <summary>
		/// Clears out _tokens of that have an expiration that has passed
		/// </summary>
		private async Task ClearTokensAsync()
		{
			_context.Tokens.RemoveRange(_context.Tokens.Where(x => x.Expired < DateTime.Now));
			await _context.SaveChangesAsync();
		}

		private DateTime NewExpired() => DateTime.Now.AddSeconds(_timeoutSeconds);
	}
}

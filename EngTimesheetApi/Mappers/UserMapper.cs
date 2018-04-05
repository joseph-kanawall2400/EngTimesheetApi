using System;
using EngTimesheetApi.Models;

namespace EngTimesheetApi.Mappers
{
	public static class UserMapper
	{
		public static User Map(AccountRegisterDTO dto)
		{
			return new User
			{
				Email = dto.Email,
				FirstName = dto.FirstName,
				LastName = dto.LastName
			};
		}

		public static User Map(UserDTO dto)
		{
			return new User
			{
				Id = dto.Id,
				Manager = dto.Manager,
				Email = dto.Email,
				FirstName = dto.FirstName,
				LastName = dto.LastName,
				Group = dto.Group,
				Registered = dto.Registered,
				Deactivated = dto.Deactivated
			};
		}

		public static UserDTO MapToUserDTO(User user)
		{
			return new UserDTO
			{
				Id = user.Id,
				Manager = user.Manager,
				Email = user.Email,
				FirstName = user.FirstName,
				LastName = user.LastName,
				Group = user.Group,
				Registered = user.Registered,
				Deactivated = user.Deactivated
			};
		}
	}
}

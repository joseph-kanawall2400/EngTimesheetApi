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
				Times = user.Times,
				Registered = user.Registered,
				Deactivated = user.Deactivated
			};
		}
	}
}

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
	}
}

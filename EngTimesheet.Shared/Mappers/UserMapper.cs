using EngTimesheet.Shared.Models;

namespace EngTimesheet.Shared.Mappers
{
	public static class UserMapper
	{
		public static User Map(AccountRegisterDTO dto) => new User
		{
			Email = dto.Email,
			FirstName = dto.FirstName,
			LastName = dto.LastName
		};

		public static User Map(UserDTO dto) => new User
		{
			Id = dto.Id,
			Manager = dto.Manager,
			Email = dto.Email,
			FirstName = dto.FirstName,
			LastName = dto.LastName,
			Registered = dto.Registered,
			Deactivated = dto.Deactivated
		};

		public static UserDTO MapToUserDTO(User user) => new UserDTO
		{
			Id = user.Id,
			Manager = user.Manager,
			Email = user.Email,
			FirstName = user.FirstName,
			LastName = user.LastName,
			Registered = user.Registered,
			Deactivated = user.Deactivated
		};
	}
}

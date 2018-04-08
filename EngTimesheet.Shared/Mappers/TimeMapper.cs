using EngTimesheet.Shared.Models;
using System.Collections.Generic;
using System.Linq;

namespace EngTimesheet.Shared.Mappers
{
	public static class TimeMapper
	{
		public static Time Map( TimeDTO dto, Time time)
		{
			time.Amount = dto.Amount;
			time.Category = dto.Category;
			time.Date = dto.Date;
			return time;
		}

		public static Time Map(TimeDTO dto) => new Time
		{
			Amount = dto.Amount,
			Category = dto.Category,
			Date = dto.Date
		};

		public static TimeDTO MapToTimeDTO(Time time) => new TimeDTO
		{
			Amount = time.Amount,
			Category = time.Category,
			Date = time.Date
		};

		public static IEnumerable<TimeDTO> MapToTimeDTO(User user) => MapToTimeDTO(user.Times);

		public static IEnumerable<TimeDTO> MapToTimeDTO(IEnumerable<Time> times) => times.Select(x => new TimeDTO
		{
			Amount = x.Amount,
			Category = x.Category,
			Date = x.Date
		});
	}
}

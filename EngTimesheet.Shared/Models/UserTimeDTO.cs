using System.Collections.Generic;

namespace EngTimesheet.Shared.Models
{
	public class UserTimeDTO
	{
		public UserDTO User { get; set; }
		public List<TimeDTO> Times { get; set; }
	}
}

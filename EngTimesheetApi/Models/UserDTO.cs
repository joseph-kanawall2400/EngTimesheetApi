using System;

namespace EngTimesheetApi.Models
{
	public class UserDTO
	{
		public int Id { get; set; }
		public bool Manager { get; set; }
		public string Email { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public DateTime? Registered { get; set; }
		public DateTime? Deactivated { get; set; }
	}
}

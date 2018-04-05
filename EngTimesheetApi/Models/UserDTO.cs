using System;
using System.Collections.Generic;

namespace EngTimesheetApi.Models
{
	public class UserDTO
	{
		public int Id { get; set; }
		public bool Manager { get; set; }
		public string Email { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public Group Group { get; set; }
		public List<Time> Times { get; set; }
		public DateTime? Registered { get; set; }
		public DateTime? Deactivated { get; set; }
	}
}

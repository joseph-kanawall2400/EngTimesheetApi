using EngTimesheetApi.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EngTimesheetApi.Models
{
	public class User
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		public bool Manager { get; set; }
		public string Email { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public Group Group { get; set; }
		public List<Time> Times { get; set; }

		private DateTime? _registered = null;
		public DateTime? Registered
		{
			get => _registered;
			set => _registered = value?.FirstOfMonth();
		}

		private DateTime? _deactivated = null;
		public DateTime? Deactivated
		{
			get => _deactivated;
			set => _deactivated = value?.FirstOfMonth();
		}
	}
}

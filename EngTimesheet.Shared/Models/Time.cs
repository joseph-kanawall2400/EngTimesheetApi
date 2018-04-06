using System;

namespace EngTimesheetApi.Shared.Models
{
	public class Time
	{
		public int Id { get; set; }
		public int Hours { get; set; }
		public TimeCategory Category { get; set; }
		public User User { get; set; }

		private DateTime _created;
		public DateTime Created
		{
			get => _created;
			set => _created = value.FirstOfMonth();
		}
	}
}

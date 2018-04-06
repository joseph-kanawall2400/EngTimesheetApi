using System;

namespace EngTimesheetApi.Models
{
	public class Time
	{
		public int Id { get; set; }
		public int Hours { get; set; }
		public TimeCategory Category { get; set; }
		public User User { get; set; }

		private DateTime _deactivated;
		public DateTime Deactivated
		{
			get => _deactivated;
			set => _deactivated = value.FirstOfMonth();
		}
	}
}

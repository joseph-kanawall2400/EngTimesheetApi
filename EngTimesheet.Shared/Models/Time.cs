using System;

namespace EngTimesheet.Shared.Models
{
	public class Time
	{
		public int Id { get; set; }
		public double Amount { get; set; }
		public TimeCategory Category { get; set; }
		public User User { get; set; }

		private DateTime _date;
		public DateTime Date
		{
			get => _date;
			set => _date = value.FirstOfMonth();
		}
	}
}

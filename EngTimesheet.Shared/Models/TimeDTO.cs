using System;

namespace EngTimesheet.Shared.Models
{
	public class TimeDTO
	{
		public double Amount { get; set; }
		public TimeCategory Category { get; set; }
		public DateTime Date { get; set; }
	}
}

using System;

namespace EngTimesheet.Shared.Models
{
	public class Token
	{
		public int Id { get; set; }
		public bool SingleUse { get; set; }
		public string Value { get; set; }
		public DateTime Expired { get; set; }
		public TokenServiceType Type { get; set; }
		public User User { get; set; }
	}
}

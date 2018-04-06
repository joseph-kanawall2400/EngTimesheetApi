using System;
using System.ComponentModel;
using System.Linq;

namespace EngTimesheetApi.Shared
{
	public static class Extensions
	{
		public static DateTime FirstOfMonth(this DateTime date) => new DateTime(date.Year, date.Month, 1);

		public static string Pretty(this Enum e) => ((DescriptionAttribute[])e.GetType().GetField(e.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false)).FirstOrDefault()?.Description ?? e.ToString();
	}
}

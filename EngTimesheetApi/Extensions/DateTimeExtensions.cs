using System;

namespace EngTimesheetApi.Extensions
{
	public static class DateTimeExtensions
	{
		public static DateTime FirstOfMonth(this DateTime date) => new DateTime(date.Year, date.Month, 1);
	}
}

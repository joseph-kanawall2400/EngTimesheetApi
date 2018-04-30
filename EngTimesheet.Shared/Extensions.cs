using EngTimesheet.Shared.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace EngTimesheet.Shared
{
	public static class Extensions
	{
		public static DateTime FirstOfMonth(this DateTime date) => new DateTime(date.Year, date.Month, 1);

		public static string Pretty(this Enum e) => ((DescriptionAttribute[])e.GetType().GetField(e.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false)).FirstOrDefault()?.Description ?? e.ToString();

		public static double Sum(this List<UserTimeDTO> users, TimeCategory category) => users.Sum(x => x.Times.Amount(category));

		public static double Percent(this List<UserTimeDTO> users, TimeCategory category)
		{
			double divisor = users.Sum(x => x.Times.Sum(t => t.Amount));
			return divisor == 0 ? 0 : users.Sum(category) / divisor;
		}

		public static double Amount(this List<TimeDTO> times, TimeCategory category) => times.SingleOrDefault(x => x.Category == category)?.Amount ?? 0;

		public static double Percent(this List<TimeDTO> times, TimeCategory category)
		{
			double divisor = times.Sum(x => x.Amount);
			return divisor == 0 ? 0 : times.Amount(category) / divisor;
		}

		private static readonly TimeCategory[] capitalizedCategories = new[] { TimeCategory.Maintenance, TimeCategory.ResearchAndDevelopment, TimeCategory.Management };

		public static double Expensed(this List<TimeDTO> times) => times.Where(x => !capitalizedCategories.Contains(x.Category)).Sum(x => x.Amount);

		public static double Capitalized(this List<TimeDTO> times) => times.Where(x => capitalizedCategories.Contains(x.Category)).Sum(x => x.Amount);
	}
}

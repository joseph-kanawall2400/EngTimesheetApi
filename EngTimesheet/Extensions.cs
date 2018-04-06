using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace EngTimesheet
{
	public static class Extensions
	{
		public static string Pretty<T>(this IEnumerable<T> list) => $"[{String.Join(", ", list)}]";
	}
}

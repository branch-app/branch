using System;

namespace Branch.Packages.Extensions
{
	public static class DateTimeExtensions
	{
		public static string ToISOString(this DateTime dt)
		{
			return dt.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss.fffK");
		}
	}
}

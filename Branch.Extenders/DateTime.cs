using System;

namespace Branch.Extenders
{
	public static class DateTimeExtenders
	{
		public static int ToUnixTimestamp(this DateTime dateTime)
		{
			return Convert.ToInt32((dateTime - new DateTime(1970, 1, 1).ToLocalTime()).TotalSeconds);
		}
	}
}

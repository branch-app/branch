using System;

namespace Branch.Extenders
{
	public static class DateTimeOffsetExtenders
	{
		public static DateTime ToDateTime(this DateTimeOffset dateTime)
		{
			if (dateTime.Offset.Equals(TimeSpan.Zero))
				return dateTime.UtcDateTime;

			return dateTime.Offset.Equals(TimeZoneInfo.Local.GetUtcOffset(dateTime.DateTime))
				? DateTime.SpecifyKind(dateTime.DateTime, DateTimeKind.Local)
				: dateTime.DateTime;
		}
	}
}

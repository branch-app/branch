using System.Globalization;

namespace Branch.Helpers.Extenders
{
	public static class StringExtenders
	{
		/// <summary>
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string ToTitleCase(this string value)
		{
			return new CultureInfo("en-US", false).TextInfo.ToTitleCase(value);
		}
	}
}
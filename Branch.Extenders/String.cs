using System.Globalization;
using System.Text.RegularExpressions;

namespace Branch.Extenders
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

		/// <summary>
		/// 
		/// </summary>
		/// <param name="phrase"></param>
		/// <param name="maxLength"></param>
		/// <returns></returns>
		public static string ToSlug(this string phrase, int maxLength = 50)
		{
			var str = phrase.ToLower();
			// invalid chars, make into spaces
			str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
			// convert multiple spaces/hyphens into one space
			str = Regex.Replace(str, @"[\s-]+", " ").Trim();
			// cut and trim it
			str = str.Substring(0, str.Length <= maxLength ? str.Length : maxLength).Trim();
			// hyphens
			str = Regex.Replace(str, @"\s", "-");

			return str;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string MakeCamelCaseFriendly(this string value)
		{
			return Regex.Replace(value, @"(\B[A-Z]+?(?=[A-Z][^A-Z])|\B[A-Z]+?(?=[^A-Z]))", " $1");
		}
	}
}
using System.Text;
using System.Text.RegularExpressions;

namespace Branch.Global.Extensions
{
	public static class StringExtensions
	{
		public static string ToSlug(this string str)
		{
			// Lifted from: https://stackoverflow.com/questions/2920744/url-slugify-algorithm-in-c

			// First to lower case
			var slug = str.ToLowerInvariant();

			// Remove all accents
			var bytes = Encoding.UTF8.GetBytes(slug);
			slug = Encoding.ASCII.GetString(bytes);

			// Replace spaces
			slug = Regex.Replace(slug, @"\s", "-", RegexOptions.Compiled);

			// Remove invalid chars
			slug = Regex.Replace(slug, @"[^a-z0-9\s-_]", "",RegexOptions.Compiled);

			// Trim dashes from end
			slug = slug.Trim('-', '_');

			// Replace double occurrences of - or _
			slug = Regex.Replace(slug, @"([-_]){2,}", "$1", RegexOptions.Compiled);

			return slug;
		}
	}
}

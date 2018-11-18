using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Branch.Packages.Extensions
{
	public static class String
	{
		// white space, em-dash, en-dash, underscore
		static readonly Regex WordDelimiters = new Regex(@"[\s—–_]", RegexOptions.Compiled);

		// characters that are not valid
		static readonly Regex InvalidChars = new Regex(@"[^a-z0-9\-]", RegexOptions.Compiled);

		// multiple hyphens
		static readonly Regex MultipleHyphens = new Regex(@"-{2,}", RegexOptions.Compiled);

		public static string ToSnakeCase(this string str)
		{
			return string.Concat(str.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString() : x.ToString())).ToLower();
		}

		/// <summary>
		/// Converts a string to a URL slug
		/// </summary>
		/// <param name="value">String to sluggify</param>
		public static string ToSlug(this string value)
		{
			var output = value;

			// convert to lower case
			output = output.ToLowerInvariant();

			// remove diacritics (accents)
			output = RemoveDiacritics(output);

			// ensure all word delimiters are hyphens
			output = WordDelimiters.Replace(output, "-");

			// strip out invalid characters
			output = InvalidChars.Replace(output, "");

			// replace multiple hyphens (-) with a single hyphen
			output = MultipleHyphens.Replace(output, "-");

			// trim hyphens (-) from ends
			return output.Trim('-');
		}

		/// See: http://www.siao2.com/2007/05/14/2629747.aspx
		private static string RemoveDiacritics(string stIn)
		{
			var stFormD = stIn.Normalize(NormalizationForm.FormD);
			var sb = new StringBuilder();

			for (int ich = 0; ich < stFormD.Length; ich++)
			{
				var uc = CharUnicodeInfo.GetUnicodeCategory(stFormD[ich]);
				if (uc != UnicodeCategory.NonSpacingMark)
					sb.Append(stFormD[ich]);
			}

			return (sb.ToString().Normalize(NormalizationForm.FormC));
		}
	}
}

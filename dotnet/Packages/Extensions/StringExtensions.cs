using System.Linq;

namespace Branch.Packages.Extensions
{
	public static class String
	{
		public static string ToSnakeCase(this string str) {
			return string.Concat(str.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString() : x.ToString())).ToLower();
		}
	}
}

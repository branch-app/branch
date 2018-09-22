using System.Text;

namespace Branch.Packages.Extensions
{
	public static class ByteArrayExtensions
	{
		/// <summary>
		/// Converts a byte array into a hex string.
		/// </summary>
		/// <param name="arr">The byte array to convert.</param>
		/// <param name="uppercase">If you want a uppercase hex string. Defaults to false.</param>
		/// <returns>The hex string.</returns>
		public static string ToHexString(this byte[] arr, bool uppercase = false)
		{
			var sb = new StringBuilder();

			for (var i = 0; i < arr.Length; i++)
				sb.Append(arr[i].ToString(uppercase ? "X2" : "x2"));

			return sb.ToString();
		}
	}
}

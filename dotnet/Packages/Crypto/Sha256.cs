using System.IO;
using System.Security.Cryptography;

namespace Branch.Packages.Crypto
{
	public static class Sha256
	{
		/// <summary>
		/// Creates a SHA256 hash based on an input stream.
		/// </summary>
		/// <param name="stream">The stream of the data to hash.</param>
		/// <returns>The hash.</returns>
		public static byte[] HashContent(Stream stream)
		{
			using (var sha256 = SHA256Managed.Create())
			{
				return sha256.ComputeHash(stream);
			}
		}
	}
}

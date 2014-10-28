using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Branch.Models.Services.Branch;

namespace Branch.Core.Cryptography
{
	public class Pbkdf2Crypto
	{
		// The following constants may be changed without breaking existing hashes.
		private const int SaltBytes = 24;
		private const int HashBytes = 24;

		/// <summary>
		/// Creates a salted PBKDF2 hash of the password.
		/// </summary>
		/// <param name="password">The password to hash.</param>
		/// <param name="iterations">The number of iterations.</param>
		/// <returns>The hash of the password.</returns>
		public static Pbkdf2Response ComputeHash(string password, int iterations)
		{
			// Generate a random salt
			var csprng = new RNGCryptoServiceProvider();
			var salt = new byte[SaltBytes];
			csprng.GetBytes(salt);

			// Hash the password and encode the parameters
			var hash = Pbkdf2(password, salt, iterations, HashBytes);
			return new Pbkdf2Response
			{
				Hash = Convert.ToBase64String(hash),
				Salt = Convert.ToBase64String(salt),
				Iterations = iterations
			};
		}

		/// <summary>
		/// Validates a password given a hash of the correct one.
		/// </summary>
		/// <param name="data">The password to check.</param>
		/// <param name="storedHash">A hash of the correct password.</param>
		/// <param name="salt">The salt used to make the hash.</param>
		/// <param name="iterations">The number of iterations.</param>
		/// <returns>True if the password is correct. False otherwise.</returns>
		public static bool ValidateHash(string data, string storedHash, string salt, int iterations)
		{
			// Extract the parameters from the hash
			var hash = Convert.FromBase64String(storedHash);
			var realSalt = Convert.FromBase64String(salt);

			var testHash = Pbkdf2(data, realSalt, iterations, hash.Length);
			return SlowEquals(hash, testHash);
		}

		/// <summary>
		/// Compares two byte arrays in length-constant time. This comparison
		/// method is used so that password hashes cannot be extracted from
		/// on-line systems using a timing attack and then attacked off-line.
		/// </summary>
		/// <param name="a">The first byte IList.</param>
		/// <param name="b">The second byte IList.</param>
		/// <returns>True if both byte arrays are equal. False otherwise.</returns>
		private static bool SlowEquals(IList<byte> a, IList<byte> b)
		{
			var diff = (uint)a.Count ^ (uint)b.Count;
			for (var i = 0; i < a.Count && i < b.Count; i++)
				diff |= (uint)(a[i] ^ b[i]);
			return diff == 0;
		}

		/// <summary>
		/// Computes the PBKDF2-SHA1 hash of a password.
		/// </summary>
		/// <param name="password">The password to hash.</param>
		/// <param name="salt">The salt.</param>
		/// <param name="iterations">The PBKDF2 iteration count.</param>
		/// <param name="outputBytes">The length of the hash to generate, in bytes.</param>
		/// <returns>A hash of the password.</returns>
		private static byte[] Pbkdf2(string password, byte[] salt, int iterations, int outputBytes)
		{
			var pbkdf2 = new Rfc2898DeriveBytes(password, salt)
			{
				IterationCount = iterations
			};
			return pbkdf2.GetBytes(outputBytes);
		}
	}
}
using System;

namespace Branch.Extenders
{
	public static class Int
	{
		public static string Ordinal(this int number)
		{
			var ones = number % 10;
			var tens = Math.Floor(number / 10f) % 10;
			if (Math.Abs(tens - 1) < 0.2)
				return number + "th";

			switch (ones)
			{
				case 1: return number + "st";
				case 2: return number + "nd";
				case 3: return number + "rd";
				default: return number + "th";
			}
		}
	}
}

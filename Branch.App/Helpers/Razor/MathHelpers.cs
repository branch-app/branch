using System;
using System.Linq;

namespace Branch.App.Helpers.Razor
{
	public class MathHelpers
	{
		public static double CalculateKd(int kills, int deaths, int roundTo = 2)
		{
			if (deaths <= 0)
				return kills;

			var ratio = (float) kills / deaths;
			return Math.Round(Convert.ToDouble(ratio), 2, MidpointRounding.AwayFromZero);
		}

		public static int CalculateSpread(int kills, int deaths, int[] otherDeathTypes)
		{
			return (kills - (deaths + otherDeathTypes.Sum()));
		}

		public static double RoundTo(double number, int roundTo)
		{
			return Math.Round(number, roundTo);
		}
	}
}
using System;
using System.Linq;

namespace Branch.App.Helpers.Razor
{
	public class MathHelpers
	{
		public static double CalculateKd(int kills, int deaths, int roundTo = 2)
		{
			return deaths <= 0 ? kills : Math.Round((double) (kills/deaths), roundTo);
		}

		public static int CalculateSpread(int kills, int deaths, int[] otherDeathTypes)
		{
			return (kills - (deaths + otherDeathTypes.Sum()));
		}
	}
}
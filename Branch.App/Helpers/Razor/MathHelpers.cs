using System;

namespace Branch.App.Helpers.Razor
{
	public class MathHelpers
	{
		public static double CalculateKd(int kills, int deaths, int roundTo)
		{
			return deaths <= 0 ? kills : Math.Round((double) (kills/deaths), roundTo);
		}
	}
}
using Branch.Core.Game.HaloReach.Api;

namespace Branch.App.Helpers.Razor.Reach
{
	public static class GeneralHelpers
	{
		public static int GetRankStartXp(string rankName)
		{
			return Manager.RankStartXpDictionary[rankName.ToLowerInvariant()];
		}
	}
}
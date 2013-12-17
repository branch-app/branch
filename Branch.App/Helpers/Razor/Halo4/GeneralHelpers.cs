using System.Globalization;
using Branch.Models.Services.Halo4._343.DataModels;

namespace Branch.App.Helpers.Razor.Halo4
{
	public class GeneralHelpers
	{
		public static int GetHighestCsr(CommonModels.CurrentSkillRank topSkillRank)
		{
			if (topSkillRank == null || topSkillRank.CompetitiveSkillRank == null)
				return 0;

			return (int) topSkillRank.CompetitiveSkillRank;
		}

		public static string GetCsrLevelUrl(int csrLevel, string size = "medium", string version = "v1")
		{
			return string.Format("https://assets.halowaypoint.com/games/h4/csr/{0}/{1}/{2}.png", version, size, csrLevel);
		}

		public static string GetRawAssetUrl(CommonModels.ImageUrl imageUrl, string size = "medium")
		{
			return string.Format("{0}{1}", GlobalStorage.H4WaypointManager.RegisteredWebApp.Settings[imageUrl.BaseUrl],
				imageUrl.AssetUrl.Replace("{size}", size));
		}

		public static string GetRawAssetUrl(CommonModels.ImageUrl imageUrl, int size = 100)
		{
			return string.Format("{0}{1}", GlobalStorage.H4WaypointManager.RegisteredWebApp.Settings[imageUrl.BaseUrl],
				imageUrl.AssetUrl.Replace("{size}", size.ToString(CultureInfo.InvariantCulture)));
		}

		public static string GetPlayerModelUrl(string gamertag, string size = "large", string pose = "fullbody")
		{
			return GlobalStorage.H4WaypointManager.GetPlayerModelUrl(gamertag, size, pose);
		}
	}
}

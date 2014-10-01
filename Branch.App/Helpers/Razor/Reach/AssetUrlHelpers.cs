using System;
using System.Linq;
using System.Text.RegularExpressions;
using Branch.Core.Game.HaloReach.Api;
using Branch.Core.Game.HaloReach.Enums;
using Branch.Core.Game.HaloReach.Helpers;

namespace Branch.App.Helpers.Razor.Reach
{
	public static class AssetUrlHelpers
	{
		public static string GetRankUrl(string path, AssetSize assetSize)
		{
			path = path.Replace("/images/reachstats/", "");

			switch (assetSize)
			{
				case AssetSize.Large:
					path = path.Replace("/med/", "/large/");
					break;

				case AssetSize.Small:
					path = path.Replace("/med/", "/sm/");
					break;
			}

			return String.Format(Manager.ApiAssetUrl, path);
		}

		public static string GetEmblemUrl(string path, int size = 120)
		{
			// replace size
			var rgx = new Regex("(s=\\d+)");
			path = rgx.Replace(path, String.Format("s={0}", size));

			// set background to be nope
			rgx = new Regex("(bi=\\d+)");
			path = rgx.Replace(path, "bi=0");

			// mtlr
			return String.Format(Manager.ApiEmblemAssetUrl, path);
		}

		public static string GetWeaponUrl(string weaponImageName)
		{
			var path = String.Format("weapons/{0}.png", weaponImageName);
			return String.Format(Manager.ApiAssetUrl, path);
		}

		public static string GetMedalUrl(string medalImageName)
		{
			var path = String.Format("Medals/{0}.png", medalImageName);
			return String.Format("/Content/Images/Area/Reach/Assets/{0}", path);
		}

		public static string GetEnemyUrl(string enemyImageName)
		{
			var path = String.Format("Enemies/{0}.png", enemyImageName);
			return String.Format("/Content/Images/Area/Reach/Assets/{0}", path);
		}

		public static string GetPlayerModelUrl(string gamertag, AssetSize assetSize)
		{
			return String.Format("https://spartans.svc.halowaypoint.com/players/{0}/Reach/spartans/fullbody?target={1}", gamertag, assetSize);
		}

		public static string GetMapImageUrl(string mapName, AssetSize assetSize = AssetSize.Large)
		{
			var path = String.Format("Maps/{0}/{1}.jpg", assetSize, AssetHelpers.MapIdDictionary[mapName]);
			return String.Format("/Content/Images/Area/Reach/Assets/{0}", path);
		}

		public static string GetMapImageUrl(int mapId, AssetSize assetSize = AssetSize.Large)
		{
			var map = GlobalStorage.HReachManager.Metadata.Data.Maps.FirstOrDefault(m => m.Id == mapId);
			var path = String.Format("Maps/{0}/{1}.jpg", assetSize, map.MapDetails.ImageName);
			return String.Format("/Content/Images/Area/Reach/Assets/{0}", path);
		}

		public static string GetIconImageUrl(GameVariantIcon icon, AssetSize assetSize)
		{
			var path = String.Format("Gametypes/{0}/{1}.png", assetSize, (int) icon);
			return String.Format("/Content/Images/Area/Reach/Assets/{0}", path);
		}

		public static string GetCommendationImageUrl(int id, string tier, CommendationVariantClass variantClass, AssetSize assetSize)
		{
			if (tier.ToLowerInvariant() == "beginner") 
				tier = "none";

			var path = String.Format("commendations/{0}/{0}_{1}_{2}_{3}.png", tier.ToLowerInvariant(),
				assetSize.ToString().ToLowerInvariant(), id, variantClass.ToString().ToLowerInvariant());
			return String.Format(Manager.ApiAssetUrl, path);
		}

		public static string GetPrivateAuthorImageUrl(string author)
		{
			var path = String.Format("PrivateAuthors/{0}_author.png", author);
			return String.Format("/Content/Images/Area/Reach/Assets/{0}", path);
		}

		public static string GetDifficultyImageUrl(Difficulty difficulty, AssetSize assetSize)
		{
			var path = String.Format("campaign_progress/{0}_{1}.png", difficulty.ToString().ToLowerInvariant(), assetSize.ToString().ToLowerInvariant());
			return String.Format(Manager.ApiAssetUrl, path);
		}
	}
}
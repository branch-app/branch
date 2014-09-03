using System;
using System.Text.RegularExpressions;
using Branch.Core.Game.HaloReach.Api;
using Branch.Core.Game.HaloReach.Enums;

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

		public static string GetPlayerModelUrl(string gamertag, AssetSize assetSize)
		{
			return String.Format("https://spartans.svc.halowaypoint.com/players/{0}/Reach/spartans/fullbody?target={1}", gamertag, assetSize);
		}
	}
}
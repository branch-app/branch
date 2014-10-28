using System;
using System.Collections.Generic;

namespace Branch.Core.Game.HaloReach.Helpers
{
	public static class AssetHelpers
	{
		public readonly static Dictionary<string, string> MapIdDictionary = new Dictionary<string, string>
		{
			// Campaign
			{ "Winter Contingency", "wintercontingency" },
			{ "ONI: Sword Base", "oni" },
			{ "Nightfall", "nightfall" },
			{ "Tip of the Spear", "tipofthespear" },
			{ "Long Night of Solace", "longnightofsolace" },
			{ "Exodus", "exodus" },
			{ "New Alexandria", "newalexandria" },
			{ "The Package", "thepackage" },
			{ "The Pillar of Autumn", "pillarofautumn" },
			{ "Lone Wolf", "lonewolf" },

			// Firefight
			{ "Overlook", "ff_overlook" },
			{ "Courtyard", "ff_courtyard" },
			{ "Outpost", "ff_outpost" },
			{ "Waterfront", "ff_waterfront" },
			{ "Beachhead", "beachhead" }, // This is the only firefight map that doesn't have a ff_ prefix.. Fucking hell.
			{ "Holdout", "ff_holdout" },
			{ "Corvette", "ff_corvette" },
			{ "Glacier", "ff_glacier" },
			{ "Unearthed", "ff_unearthed" },
			{ "Installation 04", "ff_halo" },

			// Multiplayer
			{ "Sword Base", "mp_swordbase" },
			{ "Countdown", "mp_countdown" },
			{ "Boardwalk", "mp_boardwalk" },
			{ "Zealot", "mp_zealot" },
			{ "Powerhouse", "mp_powerhouse" },
			{ "Boneyard", "mp_boneyard" },
			{ "Reflection", "mp_reflection" },
			{ "Spire", "mp_spire" },
			{ "Condemned", "mp_condemned" },
			{ "Highlands", "mp_highlands" },
			{ "Anchor 9", "mp_anchor9" },
			{ "Breakpoint", "mp_breakpoint" },
			{ "Tempest", "mp_tempest" },
			{ "Forge World", "mp_forgeworld" },
			{ "Penance", "mp_damnation" },
			{ "Battle Canyon", "mp_beaver_creek" },
			{ "Ridgeline", "mp_timberland" },
			{ "Breakneck", "mp_headlong" },
			{ "High Noon", "mp_hangemhigh" },
			{ "Solitary", "mp_prisoner" }
		};

		public static readonly Dictionary<string, int> RankStartXpDictionary = new Dictionary<string, int>
		{
			{ "recruit", 0 },
			{ "private", 7500 },
			{ "corporal", 10000 },
			{ "corporal grade 1", 15000 },
			{ "sergeant", 20000 },
			{ "sergeant grade 1", 26250 },
			{ "sergeant grade 2", 32500 },
			{ "warrant officer", 45000 },
			{ "warrant officer grade 1", 78000 },
			{ "warrant officer grade 2", 111000 },
			{ "warrant officer grade 3", 144000 },
			{ "captain", 210000 },
			{ "captain grade 1", 233000 },
			{ "captain grade 2", 256000 },
			{ "captain grade 3", 279000 },
			{ "major", 325000 },
			{ "major grade 1", 350000 },
			{ "major grade 2", 375000 },
			{ "major grade 3", 400000 },
			{ "lt. colonel", 450000 },
			{ "lt. colonel grade 1", 480000 },
			{ "lt. colonel grade 2", 510000 },
			{ "lt. colonel grade 3", 540000 },
			{ "commander", 600000 },
			{ "commander grade 1", 650000 },
			{ "commander grade 2", 700000 },
			{ "commander grade 3", 750000 },
			{ "colonel", 850000 },
			{ "colonel grade 1", 960000 },
			{ "colonel grade 2", 1070000 },
			{ "colonel grade 3", 1180000 },
			{ "brigadier", 1400000 },
			{ "brigadier grade 1", 1520000 },
			{ "brigadier grade 2", 1640000 },
			{ "brigadier grade 3", 1760000 },
			{ "general", 2000000 },
			{ "general grade 1", 2200000 },
			{ "general grade 2", 2350000 },
			{ "general grade 3", 2500000 },
			{ "general grade 4", 2650000 },
			{ "field marshall", 3000000 },
			{ "hero", 3700000 },
			{ "legend", 4600000 },
			{ "mythic", 5650000 },
			{ "noble", 7000000 },
			{ "eclipse", 8500000 },
			{ "nova", 11000000 },
			{ "forerunner", 13000000 },
			{ "reclaimer", 16500000 },
			{ "inheritor", 20000000 }
		};

		public static readonly Dictionary<int, Tuple<string, string>> TeamColourDictionary = new Dictionary<int, Tuple<string, string>>
		{
			{ -1, new Tuple<string, string>("#3d3d7c", "Players") },
			{ 0, new Tuple<string, string>("#D96551", "Red Team") },
			{ 1, new Tuple<string, string>("#6384B0", "Blue Team") },
			{ 2, new Tuple<string, string>("#00A400", "Green Team") },
			{ 3, new Tuple<string, string>("#FCB15E", "Orange Team") },
			{ 4, new Tuple<string, string>("#B579E4", "Purple Team") },
			{ 5, new Tuple<string, string>("#FCE000", "Gold Team") },
			{ 6, new Tuple<string, string>("#9F7757", "Brown Team") },
			{ 7, new Tuple<string, string>("#FFBAE2", "Pink Team") }
		};
	}
}

using System;
using Branch.Core.Game.HaloReach.Enums;
using Branch.Core.Game.HaloReach.JsonConverters;
using Newtonsoft.Json;

namespace Branch.Core.Game.HaloReach.Models._343.DataModels
{
	public class File
	{
		public string Author { get; set; }

		public string OriginalAuthor { get; set; }

		public string Title { get; set; }

		public string Description { get; set; }

		public DateTime CreateDate { get; set; }

		public DateTime ModifiedDate { get; set; }

		[JsonConverter(typeof(FileTypeConverter))]
		public FileType FileCategory { get; set; }

		public long FileId { get; set; }

		public long MapId { get; set; }

		public UInt64 LikesCount { get; set; }

		public UInt64 DownloadCount { get; set; }

		public bool OffensiveToBeHidden { get; set; }

		public string ScreenshotFullSizeUrl { get; set; }

		public string ScreenshotMediumUrl { get; set; }

		public string ScreenshotThumbnailUrl { get; set; }
	}
}

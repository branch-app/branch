using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Branch.Models.Helpers;

namespace Branch.Models.Sql
{
	public class BranchSession
		: Audit
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public Guid Identifier { get; set; }

		[Required]
		public string Ip { get; set; }

		[Required]
		public string FriendlyLocation { get; set; }

		[Required]
		public string GpsLocation { get; set; }

		[Required]
		public string UserAgent { get; set; }

		[Required]
		public string Platform { get; set; }

		[Required]
		public string Browser { get; set; }

		[Required]
		public bool Revoked { get; set; }

		[Required]
		public DateTime ExpiresAt { get; set; }

		public bool IsValid()
		{
			if (Revoked)
				return false;

			return ExpiresAt > DateTime.UtcNow;
		}

		public static BranchSession Create(string ipAddress, string userAgent, BranchIdentity branchIdentity, bool rememberMe)
		{
			var modernAgent = new ModernAgent(userAgent);

			var session = new BranchSession
			{
				Revoked = false,
				Ip = ipAddress,
				Identifier = Guid.NewGuid(),
				Browser = modernAgent.Browser(),
				Platform = modernAgent.OperatingSystem(),
				UserAgent = userAgent,
				//BranchIdentity = branchIdentity,
				ExpiresAt = rememberMe ? DateTime.UtcNow.AddYears(1) : DateTime.UtcNow.AddDays(1)
			};

			if (ipAddress == "127.0.0.1")
			{
				session.FriendlyLocation = "Unknown Location - Developer Session - Running on Local Machine";
				session.GpsLocation = String.Format("{0},{1}", 0, 0);
				return session;
			}

			var g = IpGeolocation.Geolocate(ipAddress);
			if (g.Latitude <= -1336)
			{
				session.FriendlyLocation = "Unknown Location";
				session.GpsLocation = String.Format("{0},{1}", 0, 0);
				return session;
			}

			var gg = new Geocoding.Google.GoogleGeocoder().ReverseGeocode(g.Latitude, g.Longitude).ToList();
			if (gg.Any())
			{
				var ggg = gg.First();
				session.FriendlyLocation = ggg.FormattedAddress;
				session.GpsLocation = String.Format("{0},{1}", ggg.Coordinates.Latitude, ggg.Coordinates.Longitude);
			}
			else
			{
				session.FriendlyLocation = "Unknown Location";
				session.GpsLocation = String.Format("{0},{1}", 0, 0);
			}

			return session;
		}

		public virtual BranchIdentity BranchIdentity { get; set; }
	}
}

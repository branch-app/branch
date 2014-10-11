using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Branch.Core.Storage;

namespace Branch.App.Areas.Api.Controllers
{
	public class SearchController : ApiController
	{
		public IEnumerable<object> GetIdeneities(string id)
		{
			using (var sqlStorage = new SqlStorage())
			{
				var halo4Identities =
					sqlStorage.Halo4Identities.Where(i => i.GamerIdentity.GamerIdSafe.Contains(id)).Take(4).ToList().Select(h => new
					{
						Ident = "Halo 4",
						Url = Url.Route("Halo4_ServiceRecord", new { gamertag = h.GamerIdentity.GamerId }),
						h.FavouriteWeapon,
						h.KillDeathRatio,
						h.PlayerModelUrl,
						h.TopCsr,
						h.TotalKills,
						h.GamerIdentity.GamerId,
						h.ServiceTag,
						GamerIdType = h.GamerIdentity.Type
					});
				var reachIdentities = 
					sqlStorage.ReachIdentities.Where(i => i.GamerIdentity.GamerIdSafe.Contains(id)).Take(4).ToList().Select(h => new
					{
						Ident = "Halo: Reach",
						Url = Url.Route("Reach_ServiceRecord", new { gamertag = h.GamerIdentity.GamerId }),
						h.CompetitiveKills,
						h.KillDeathRatio,
						h.PlayerModelUrl,
						h.Rank,
						h.TotalGames,
						h.GamerIdentity.GamerId,
						h.ServiceTag,
						GamerIdType = h.GamerIdentity.Type
					});

				var idents = new List<object>();
				idents.AddRange(halo4Identities);
				idents.AddRange(reachIdentities);

				return idents;
			}
		}
	}
}

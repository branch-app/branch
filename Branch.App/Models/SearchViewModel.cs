using System;
using System.Collections.Generic;
using Branch.App.Models.Enums;
using Branch.Models.Sql;
using Halo4 = Branch.Core.Game.Halo4.Models._343.Responses;
using HaloReach = Branch.Core.Game.HaloReach.Models._343.Responces;

namespace Branch.App.Models
{
	public class SearchViewModel
	{
		public SearchViewModel(string query, IEnumerable<Halo4Identity> halo4Identities, IEnumerable<ReachIdentity> reachIdentities)
		{
			Query = query;
			Halo4Identities = halo4Identities;
			ReachIdentities = reachIdentities;
		}

		public string Query { get; set; }

		public IEnumerable<Halo4Identity> Halo4Identities { get; set; }

		public IEnumerable<ReachIdentity> ReachIdentities { get; set; }
	}

	public class SearchIdentityViewModel
	{
		public SearchIdentityViewModel(SearchIdent searchIdent, IEnumerable<Halo4Identity> halo4Identities,
			IEnumerable<ReachIdentity> reachIdentities, string query, int page, bool hasMorePages)
		{
			SearchIdent = searchIdent;
			Halo4Identities = halo4Identities;
			ReachIdentities = reachIdentities;
			Query = query;

			Page = page;
			HasMorePages = hasMorePages;
		}

		public SearchIdent SearchIdent { get; set; }

		public IEnumerable<Halo4Identity> Halo4Identities { get; set; }

		public IEnumerable<ReachIdentity> ReachIdentities { get; set; }

		public String Query { get; set; }

		public int Page { get; set; }

		public bool HasMorePages { get; set; }
	}
}

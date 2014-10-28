using System;
using System.Linq;
using System.Web;
using Branch.Core.Storage;
using Branch.Models.Sql;
using System.Data.Entity;

namespace Branch.App.Extentions.Helpers
{
	public static class Authentication
	{
		public static BranchIdentity GetAuthenticatedIdentity()
		{
			using (var sqlStorage = new SqlStorage())
			{
				var sessionIdentifier = HttpContext.Current.Request.Cookies["SessionIdentifier"];
				if (sessionIdentifier == null)
					return null;

				// Validate Session
				var sessionGuid = Guid.Parse(sessionIdentifier.Value);
				var session = sqlStorage.BranchSessions
					.Include(s => s.BranchIdentity).Include(s => s.BranchIdentity.GamerIdentity)
					.Include(s => s.BranchIdentity.GamerIdentity.ReachIdentities)
					.Include(s => s.BranchIdentity.GamerIdentity.Halo4Identities)
					.FirstOrDefault(s => s.Identifier == sessionGuid);
				if (session == null)
					return null;

				// Return Branch Identity
				return session.IsValid() ? null : session.BranchIdentity;
			}
		}
	}
}

using System.Data.Entity;

namespace Branch.Models.Sql
{
	public class DatabaseContext : DbContext
	{
		public DatabaseContext()
			: base("DefaultConnection") { }

		public DbSet<Authentication> Authentications { get; set; }

		public DbSet<GamerIdentity> GamerIdentities { get; set; }

		public DbSet<Halo4Identity> Halo4Identities { get; set; }

		public DbSet<ReachIdentity> ReachIdentities { get; set; }
	}
}

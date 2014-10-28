using System.Data.Entity;

namespace Branch.Models.Sql
{
	public class DatabaseContext : DbContext
	{
		public DatabaseContext()
			: base(@"Data Source=(localdb)\ProjectsV12;Initial Catalog=BranchDevelopment;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False") { }

		static DatabaseContext()
		{
			Database.SetInitializer<DbContext>(null);
		}

		public DbSet<Authentication> Authentications { get; set; }

		public DbSet<BranchRole> BranchRoles { get; set; }

		public DbSet<BranchSession> BranchSessions { get; set; }

		public DbSet<BranchIdentity> BranchIdentities { get; set; }

		public DbSet<GamerIdentity> GamerIdentities { get; set; }

		public DbSet<Halo4Identity> Halo4Identities { get; set; }

		public DbSet<ReachIdentity> ReachIdentities { get; set; }
	}
}

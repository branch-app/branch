using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
#if REMOTE
using System.Data.Entity.Migrations;
#endif
using Branch.Models.Migrations;

namespace Branch.Models.Sql
{
	public class DatabaseContext : DbContext
	{
#if REMOTE	// Running on Azure

		public DatabaseContext()
			: base(@"Server=tcp:kvfrzkb5dd.database.windows.net,1433;Database=branch-db;User ID=branch-prod-login@kvfrzkb5dd;Password=GW6GeG8jkVMBAMUu7Yd5qA4S;Trusted_Connection=False;Encrypt=True;Connection Timeout=30;")
		{
			// Migrations
			var migrator = new DbMigrator(new DbMigrationsConfiguration());
			if (migrator.GetPendingMigrations().Any())
				migrator.Update();
		}
#elif LOCAL	// Running Locally
		public DatabaseContext()
			: base(@"Data Source=(localdb)\ProjectsV12;Initial Catalog=BranchDevelopment;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False") { }
#else		// wtf?
		throw new InvalidOperationException();
#endif

		static DatabaseContext()
		{
			Database.SetInitializer(new MigrateDatabaseToLatestVersion<DatabaseContext, Configuration>());
		}

		#region Overrides & Audit

		public override int SaveChanges()
		{
			UpdateAuditInformation();
			return base.SaveChanges();
		}

		private IEnumerable<DbEntityEntry> ChangeTrackerEntries
		{
			get { return ChangeTracker.Entries().AsEnumerable(); }
		}

		private void UpdateAuditInformation()
		{
			UpdateAddedEntries();
			UpdateModifiedEntries();
		}

		private void UpdateAddedEntries()
		{
			var addedEntries = ChangeTrackerEntries.Where(e => e.State == EntityState.Added && e.Entity is Audit).Select(e => e.Entity as Audit);
			foreach (var addedEntry in addedEntries)
				addedEntry.UpdatedAt = addedEntry.CreatedAt = DateTime.UtcNow;
		}

		private void UpdateModifiedEntries()
		{
			var modifiedEntries = ChangeTrackerEntries.Where(e => e.State == EntityState.Modified && e.Entity is Audit).Select(e => e.Entity as Audit);
			foreach (var modifiedEntry in modifiedEntries)
				modifiedEntry.UpdatedAt = DateTime.UtcNow;
		}

		#endregion

		#region Database Tables

		public DbSet<Authentication> Authentications { get; set; }

		public DbSet<BranchRole> BranchRoles { get; set; }

		public DbSet<BranchSession> BranchSessions { get; set; }

		public DbSet<BranchIdentity> BranchIdentities { get; set; }

		public DbSet<BranchIdentityInvitation> BranchIdentityInvitations { get; set; }

		public DbSet<GamerIdentity> GamerIdentities { get; set; }

		public DbSet<Halo4Identity> Halo4Identities { get; set; }

		public DbSet<ReachIdentity> ReachIdentities { get; set; }

		#endregion
	}
}

using System.Linq;
using Branch.Models.Sql;

namespace Branch.Models.Migrations
{
	using System.Data.Entity.Migrations;

	public sealed class Configuration : DbMigrationsConfiguration<DatabaseContext>
	{
		public Configuration()
		{
			AutomaticMigrationsEnabled = false;
		}

		protected override void Seed(DatabaseContext context)
		{
			//  This method will be called after migrating to the latest version.

			if (!context.BranchRoles.Any())
				context.BranchRoles.AddOrUpdate(r => r.Name,
					new BranchRole { Name = "Spartan", Type = RoleType.User },
					new BranchRole { Name = "Monitor", Type = RoleType.Moderator },
					new BranchRole { Name = "Domain", Type = RoleType.Administrator },
					new BranchRole { Name = "The Flood", Type = RoleType.Banned }
				);
		}
	}
}

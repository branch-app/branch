using Branch.Models.Sql;

namespace Branch.Models.Migrations
{
	using System.Data.Entity.Migrations;

	internal sealed class Configuration : DbMigrationsConfiguration<DatabaseContext>
	{
		public Configuration()
		{
			AutomaticMigrationsEnabled = false;
		}

		protected override void Seed(DatabaseContext context)
		{
			context.BranchRoles.AddOrUpdate(
				r => r.Name,
				new BranchRole {Name = "Spartan", Type = RoleType.User},
				new BranchRole {Name = "Sentinal", Type = RoleType.Moderator},
				new BranchRole {Name = "Monitor", Type = RoleType.Administrator},
				new BranchRole {Name = "The Flood", Type = RoleType.Banned}
			);

			context.BranchIdentityInvitations.AddOrUpdate(
				i => i.InvitationCode,
				new BranchIdentityInvitation("fuck yo politics man"));
		}
	}
}

using System;

namespace Branch.Models.Migrations
{
	using System.Data.Entity.Migrations;

	public partial class AduitInformationtoModels : DbMigration
	{
		public override void Up()
		{
			AddColumn("dbo.Authentications", "CreatedAt", c => c.DateTime(nullable: false, defaultValue: DateTime.UtcNow));
			AddColumn("dbo.Authentications", "UpdatedAt", c => c.DateTime(nullable: false, defaultValue: DateTime.UtcNow));
			AddColumn("dbo.BranchIdentities", "CreatedAt", c => c.DateTime(nullable: false, defaultValue: DateTime.UtcNow));
			AddColumn("dbo.BranchIdentities", "UpdatedAt", c => c.DateTime(nullable: false, defaultValue: DateTime.UtcNow));
			AddColumn("dbo.BranchSessions", "CreatedAt", c => c.DateTime(nullable: false, defaultValue: DateTime.UtcNow));
			AddColumn("dbo.BranchSessions", "UpdatedAt", c => c.DateTime(nullable: false, defaultValue: DateTime.UtcNow));
			AddColumn("dbo.BranchRoles", "CreatedAt", c => c.DateTime(nullable: false, defaultValue: DateTime.UtcNow));
			AddColumn("dbo.BranchRoles", "UpdatedAt", c => c.DateTime(nullable: false, defaultValue: DateTime.UtcNow));
			AddColumn("dbo.GamerIdentities", "CreatedAt", c => c.DateTime(nullable: false, defaultValue: DateTime.UtcNow));
			AddColumn("dbo.GamerIdentities", "UpdatedAt", c => c.DateTime(nullable: false, defaultValue: DateTime.UtcNow));
			AddColumn("dbo.Halo4Identity", "CreatedAt", c => c.DateTime(nullable: false, defaultValue: DateTime.UtcNow));
			AddColumn("dbo.Halo4Identity", "UpdatedAt", c => c.DateTime(nullable: false, defaultValue: DateTime.UtcNow));
			AddColumn("dbo.ReachIdentities", "CreatedAt", c => c.DateTime(nullable: false, defaultValue: DateTime.UtcNow));
			AddColumn("dbo.ReachIdentities", "UpdatedAt", c => c.DateTime(nullable: false, defaultValue: DateTime.UtcNow));
		}

		public override void Down()
		{
			DropColumn("dbo.ReachIdentities", "UpdatedAt");
			DropColumn("dbo.ReachIdentities", "CreatedAt");
			DropColumn("dbo.Halo4Identity", "UpdatedAt");
			DropColumn("dbo.Halo4Identity", "CreatedAt");
			DropColumn("dbo.GamerIdentities", "UpdatedAt");
			DropColumn("dbo.GamerIdentities", "CreatedAt");
			DropColumn("dbo.BranchRoles", "UpdatedAt");
			DropColumn("dbo.BranchRoles", "CreatedAt");
			DropColumn("dbo.BranchSessions", "UpdatedAt");
			DropColumn("dbo.BranchSessions", "CreatedAt");
			DropColumn("dbo.BranchIdentities", "UpdatedAt");
			DropColumn("dbo.BranchIdentities", "CreatedAt");
			DropColumn("dbo.Authentications", "UpdatedAt");
			DropColumn("dbo.Authentications", "CreatedAt");
		}
	}
}

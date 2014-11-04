using System;

namespace Branch.Models.Migrations
{
	using System.Data.Entity.Migrations;

	public partial class RemoveExampleDefaultSQLValue : DbMigration
	{
		public override void Up()
		{
			AlterColumn("dbo.Authentications", "CreatedAt", c => c.DateTime(nullable: false));
			AlterColumn("dbo.Authentications", "UpdatedAt", c => c.DateTime(nullable: false));
			AlterColumn("dbo.BranchIdentities", "CreatedAt", c => c.DateTime(nullable: false));
			AlterColumn("dbo.BranchIdentities", "UpdatedAt", c => c.DateTime(nullable: false));
			AlterColumn("dbo.BranchSessions", "CreatedAt", c => c.DateTime(nullable: false));
			AlterColumn("dbo.BranchSessions", "UpdatedAt", c => c.DateTime(nullable: false));
			AlterColumn("dbo.BranchRoles", "CreatedAt", c => c.DateTime(nullable: false));
			AlterColumn("dbo.BranchRoles", "UpdatedAt", c => c.DateTime(nullable: false));
			AlterColumn("dbo.GamerIdentities", "CreatedAt", c => c.DateTime(nullable: false));
			AlterColumn("dbo.GamerIdentities", "UpdatedAt", c => c.DateTime(nullable: false));
			AlterColumn("dbo.Halo4Identity", "CreatedAt", c => c.DateTime(nullable: false));
			AlterColumn("dbo.Halo4Identity", "UpdatedAt", c => c.DateTime(nullable: false));
			AlterColumn("dbo.ReachIdentities", "CreatedAt", c => c.DateTime(nullable: false));
			AlterColumn("dbo.ReachIdentities", "UpdatedAt", c => c.DateTime(nullable: false));
		}

		public override void Down()
		{
			AlterColumn("dbo.Authentications", "CreatedAt", c => c.DateTime(nullable: false, defaultValue: DateTime.UtcNow));
			AlterColumn("dbo.Authentications", "UpdatedAt", c => c.DateTime(nullable: false, defaultValue: DateTime.UtcNow));
			AlterColumn("dbo.BranchIdentities", "CreatedAt", c => c.DateTime(nullable: false, defaultValue: DateTime.UtcNow));
			AlterColumn("dbo.BranchIdentities", "UpdatedAt", c => c.DateTime(nullable: false, defaultValue: DateTime.UtcNow));
			AlterColumn("dbo.BranchSessions", "CreatedAt", c => c.DateTime(nullable: false, defaultValue: DateTime.UtcNow));
			AlterColumn("dbo.BranchSessions", "UpdatedAt", c => c.DateTime(nullable: false, defaultValue: DateTime.UtcNow));
			AlterColumn("dbo.BranchRoles", "CreatedAt", c => c.DateTime(nullable: false, defaultValue: DateTime.UtcNow));
			AlterColumn("dbo.BranchRoles", "UpdatedAt", c => c.DateTime(nullable: false, defaultValue: DateTime.UtcNow));
			AlterColumn("dbo.GamerIdentities", "CreatedAt", c => c.DateTime(nullable: false, defaultValue: DateTime.UtcNow));
			AlterColumn("dbo.GamerIdentities", "UpdatedAt", c => c.DateTime(nullable: false, defaultValue: DateTime.UtcNow));
			AlterColumn("dbo.Halo4Identity", "CreatedAt", c => c.DateTime(nullable: false, defaultValue: DateTime.UtcNow));
			AlterColumn("dbo.Halo4Identity", "UpdatedAt", c => c.DateTime(nullable: false, defaultValue: DateTime.UtcNow));
			AlterColumn("dbo.ReachIdentities", "CreatedAt", c => c.DateTime(nullable: false, defaultValue: DateTime.UtcNow));
			AlterColumn("dbo.ReachIdentities", "UpdatedAt", c => c.DateTime(nullable: false, defaultValue: DateTime.UtcNow));
		}
	}
}

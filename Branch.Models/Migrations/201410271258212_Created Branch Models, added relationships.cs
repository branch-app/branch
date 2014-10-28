namespace Branch.Models.Migrations
{
	using System.Data.Entity.Migrations;

	public partial class CreatedBranchModelsaddedrelationships : DbMigration
	{
		public override void Up()
		{
			CreateTable(
				"dbo.BranchIdentities",
				c => new
					{
						Id = c.Int(nullable: false, identity: true),
						Username = c.String(nullable: false),
						PasswordHash = c.String(nullable: false),
						PasswordSalt = c.String(nullable: false),
						PasswordIterations = c.Int(nullable: false),
						Email = c.String(nullable: false),
						FullName = c.String(nullable: false),
						BranchRole_Id = c.Int(),
						GamerIdentity_Id = c.Int(),
					})
				.PrimaryKey(t => t.Id)
				.ForeignKey("dbo.BranchRoles", t => t.BranchRole_Id)
				.ForeignKey("dbo.GamerIdentities", t => t.GamerIdentity_Id)
				.Index(t => t.BranchRole_Id)
				.Index(t => t.GamerIdentity_Id);

			CreateTable(
				"dbo.BranchSessions",
				c => new
					{
						Id = c.Int(nullable: false, identity: true),
						Identifier = c.Guid(nullable: false),
						Ip = c.String(nullable: false),
						FriendlyLocation = c.String(nullable: false),
						GpsLocation = c.String(nullable: false),
						UserAgent = c.String(nullable: false),
						Platform = c.String(nullable: false),
						Browser = c.String(nullable: false),
						Revoked = c.Boolean(nullable: false),
						ExpiresAt = c.DateTime(nullable: false),
						BranchIdentity_Id = c.Int(),
					})
				.PrimaryKey(t => t.Id)
				.ForeignKey("dbo.BranchIdentities", t => t.BranchIdentity_Id)
				.Index(t => t.BranchIdentity_Id);

			CreateTable(
				"dbo.BranchRoles",
				c => new
					{
						Id = c.Int(nullable: false, identity: true),
						Name = c.String(nullable: false),
						Type = c.Int(nullable: false),
					})
				.PrimaryKey(t => t.Id);

		}

		public override void Down()
		{
			DropForeignKey("dbo.BranchIdentities", "GamerIdentity_Id", "dbo.GamerIdentities");
			DropForeignKey("dbo.BranchIdentities", "BranchRole_Id", "dbo.BranchRoles");
			DropForeignKey("dbo.BranchSessions", "BranchIdentity_Id", "dbo.BranchIdentities");
			DropIndex("dbo.BranchSessions", new[] { "BranchIdentity_Id" });
			DropIndex("dbo.BranchIdentities", new[] { "GamerIdentity_Id" });
			DropIndex("dbo.BranchIdentities", new[] { "BranchRole_Id" });
			DropTable("dbo.BranchRoles");
			DropTable("dbo.BranchSessions");
			DropTable("dbo.BranchIdentities");
		}
	}
}

namespace Branch.Models.Migrations
{
	using System.Data.Entity.Migrations;

	public partial class RelationshipsHowcute : DbMigration
	{
		public override void Up()
		{
			CreateTable(
				"dbo.Halo4Identity",
				c => new
					{
						Id = c.Int(nullable: false, identity: true),
						PlayerModelUrl = c.String(nullable: false),
						TotalKills = c.Int(nullable: false),
						KillDeathRatio = c.Double(nullable: false),
						TopCsr = c.Int(nullable: false),
						FavouriteWeapon = c.String(nullable: false),
						GamerIdentity_Id = c.Int(),
					})
				.PrimaryKey(t => t.Id)
				.ForeignKey("dbo.GamerIdentities", t => t.GamerIdentity_Id)
				.Index(t => t.GamerIdentity_Id);

			CreateTable(
				"dbo.ReachIdentities",
				c => new
					{
						Id = c.Int(nullable: false, identity: true),
						PlayerModelUrl = c.String(nullable: false),
						CompetitiveKills = c.Int(nullable: false),
						KillDeathRatio = c.Double(nullable: false),
						Rank = c.String(nullable: false),
						TotalGames = c.Int(nullable: false),
						GamerIdentity_Id = c.Int(),
					})
				.PrimaryKey(t => t.Id)
				.ForeignKey("dbo.GamerIdentities", t => t.GamerIdentity_Id)
				.Index(t => t.GamerIdentity_Id);

		}

		public override void Down()
		{
			DropForeignKey("dbo.ReachIdentities", "GamerIdentity_Id", "dbo.GamerIdentities");
			DropForeignKey("dbo.Halo4Identity", "GamerIdentity_Id", "dbo.GamerIdentities");
			DropIndex("dbo.ReachIdentities", new[] { "GamerIdentity_Id" });
			DropIndex("dbo.Halo4Identity", new[] { "GamerIdentity_Id" });
			DropTable("dbo.ReachIdentities");
			DropTable("dbo.Halo4Identity");
		}
	}
}

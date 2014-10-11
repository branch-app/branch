namespace Branch.Models.Migrations
{
	using System.Data.Entity.Migrations;

	public partial class AddedIdentities : DbMigration
	{
		public override void Up()
		{
			CreateTable(
				"dbo.GamerIdentities",
				c => new
					{
						Id = c.Int(nullable: false, identity: true),
						GamerId = c.String(nullable: false),
						GamerIdSafe = c.String(nullable: false),
						Type = c.Int(nullable: false),
					})
				.PrimaryKey(t => t.Id);

		}

		public override void Down()
		{
			DropTable("dbo.GamerIdentities");
		}
	}
}

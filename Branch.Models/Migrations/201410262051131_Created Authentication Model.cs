namespace Branch.Models.Migrations
{
	using System.Data.Entity.Migrations;

	public partial class CreatedAuthenticationModel : DbMigration
	{
		public override void Up()
		{
			CreateTable(
				"dbo.Authentications",
				c => new
					{
						Id = c.Int(nullable: false, identity: true),
						Type = c.Int(nullable: false),
						Key = c.String(nullable: false),
						IsValid = c.Boolean(nullable: false),
					})
				.PrimaryKey(t => t.Id);

		}

		public override void Down()
		{
			DropTable("dbo.Authentications");
		}
	}
}

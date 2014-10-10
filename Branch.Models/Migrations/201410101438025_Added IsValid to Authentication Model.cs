namespace Branch.Models.Migrations
{
	using System.Data.Entity.Migrations;

	public partial class AddedIsValidtoAuthenticationModel : DbMigration
	{
		public override void Up()
		{
			AddColumn("dbo.Authentications", "IsValid", c => c.Boolean(nullable: false));
		}

		public override void Down()
		{
			DropColumn("dbo.Authentications", "IsValid");
		}
	}
}

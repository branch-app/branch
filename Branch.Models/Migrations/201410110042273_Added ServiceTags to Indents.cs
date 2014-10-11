namespace Branch.Models.Migrations
{
	using System.Data.Entity.Migrations;

	public partial class AddedServiceTagstoIndents : DbMigration
	{
		public override void Up()
		{
			AddColumn("dbo.Halo4Identity", "ServiceTag", c => c.String(nullable: false));
			AddColumn("dbo.ReachIdentities", "ServiceTag", c => c.String(nullable: false));
		}

		public override void Down()
		{
			DropColumn("dbo.ReachIdentities", "ServiceTag");
			DropColumn("dbo.Halo4Identity", "ServiceTag");
		}
	}
}

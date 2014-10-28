namespace Branch.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixedemptyServiceTags : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Halo4Identity", "ServiceTag", c => c.String());
            AlterColumn("dbo.Halo4Identity", "FavouriteWeapon", c => c.String());
            AlterColumn("dbo.ReachIdentities", "ServiceTag", c => c.String());
            AlterColumn("dbo.ReachIdentities", "Rank", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ReachIdentities", "Rank", c => c.String(nullable: false));
            AlterColumn("dbo.ReachIdentities", "ServiceTag", c => c.String(nullable: false));
            AlterColumn("dbo.Halo4Identity", "FavouriteWeapon", c => c.String(nullable: false));
            AlterColumn("dbo.Halo4Identity", "ServiceTag", c => c.String(nullable: false));
        }
    }
}

namespace Branch.Models.Migrations
{
	using System;
	using System.Data.Entity.Migrations;

	public partial class AddedInviteModels : DbMigration
	{
		public override void Up()
		{
			CreateTable(
				"dbo.BranchIdentityInvitations",
				c => new
					{
						Id = c.Int(nullable: false, identity: true),
						Used = c.Boolean(nullable: false),
						InvitationCode = c.String(nullable: false),
						CreatedAt = c.DateTime(nullable: false),
						UpdatedAt = c.DateTime(nullable: false),
					})
				.PrimaryKey(t => t.Id);

			AddColumn("dbo.BranchIdentities", "BranchIdentityInvitation_Id", c => c.Int());
			CreateIndex("dbo.BranchIdentities", "BranchIdentityInvitation_Id");
			AddForeignKey("dbo.BranchIdentities", "BranchIdentityInvitation_Id", "dbo.BranchIdentityInvitations", "Id");
		}

		public override void Down()
		{
			DropForeignKey("dbo.BranchIdentities", "BranchIdentityInvitation_Id", "dbo.BranchIdentityInvitations");
			DropIndex("dbo.BranchIdentities", new[] { "BranchIdentityInvitation_Id" });
			DropColumn("dbo.BranchIdentities", "BranchIdentityInvitation_Id");
			DropTable("dbo.BranchIdentityInvitations");
		}
	}
}

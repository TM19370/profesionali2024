namespace DataBaseClasses.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _28021 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Measures", "account_account_id", "dbo.Accounts");
            DropIndex("dbo.Measures", new[] { "account_account_id" });
            AddColumn("dbo.Accounts", "FullName_fullName_id", c => c.Int(nullable: false));
            CreateIndex("dbo.Accounts", "FullName_fullName_id");
            AddForeignKey("dbo.Accounts", "FullName_fullName_id", "dbo.FullNames", "fullName_id", cascadeDelete: true);
            DropColumn("dbo.Measures", "account_account_id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Measures", "account_account_id", c => c.Int(nullable: false));
            DropForeignKey("dbo.Accounts", "FullName_fullName_id", "dbo.FullNames");
            DropIndex("dbo.Accounts", new[] { "FullName_fullName_id" });
            DropColumn("dbo.Accounts", "FullName_fullName_id");
            CreateIndex("dbo.Measures", "account_account_id");
            AddForeignKey("dbo.Measures", "account_account_id", "dbo.Accounts", "account_id", cascadeDelete: true);
        }
    }
}

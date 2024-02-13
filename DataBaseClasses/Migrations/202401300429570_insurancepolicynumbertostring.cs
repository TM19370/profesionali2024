namespace DataBaseClasses.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class insurancepolicynumbertostring : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Clients", "insurancePolicyNumber", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Clients", "insurancePolicyNumber", c => c.Int(nullable: false));
        }
    }
}

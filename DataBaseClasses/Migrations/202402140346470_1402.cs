namespace DataBaseClasses.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _1402 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Clients", "FullName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Clients", "FullName", c => c.String());
        }
    }
}

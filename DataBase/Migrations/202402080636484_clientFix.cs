namespace DataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class clientFix : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Clients", "photoPath", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Clients", "photoPath", c => c.String(nullable: false));
        }
    }
}

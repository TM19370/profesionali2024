namespace DataBaseClasses.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _0103 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Events",
                c => new
                    {
                        event_id = c.Int(nullable: false, identity: true),
                        name = c.String(nullable: false),
                        color = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.event_id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Events");
        }
    }
}

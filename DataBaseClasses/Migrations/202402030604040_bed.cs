namespace DataBaseClasses.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class bed : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Beds",
                c => new
                    {
                        bed_id = c.Int(nullable: false, identity: true),
                        roomNumber = c.Int(nullable: false),
                        bedCode = c.String(nullable: false),
                        Client_client_id = c.Int(),
                    })
                .PrimaryKey(t => t.bed_id)
                .ForeignKey("dbo.Clients", t => t.Client_client_id)
                .Index(t => t.Client_client_id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Beds", "Client_client_id", "dbo.Clients");
            DropIndex("dbo.Beds", new[] { "Client_client_id" });
            DropTable("dbo.Beds");
        }
    }
}

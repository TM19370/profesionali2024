namespace DataBaseClasses.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Hospitalizations", "client_client_id", "dbo.Clients");
            DropIndex("dbo.Hospitalizations", new[] { "client_client_id" });
            AlterColumn("dbo.Clients", "passportGetInfo", c => c.String(nullable: false));
            AlterColumn("dbo.Clients", "workPlace", c => c.String(nullable: false));
            AlterColumn("dbo.Clients", "insuranceCompany", c => c.String(nullable: false));
            AlterColumn("dbo.Hospitalizations", "client_client_id", c => c.Int(nullable: false));
            CreateIndex("dbo.Hospitalizations", "client_client_id");
            AddForeignKey("dbo.Hospitalizations", "client_client_id", "dbo.Clients", "client_id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Hospitalizations", "client_client_id", "dbo.Clients");
            DropIndex("dbo.Hospitalizations", new[] { "client_client_id" });
            AlterColumn("dbo.Hospitalizations", "client_client_id", c => c.Int());
            AlterColumn("dbo.Clients", "insuranceCompany", c => c.String());
            AlterColumn("dbo.Clients", "workPlace", c => c.String());
            AlterColumn("dbo.Clients", "passportGetInfo", c => c.String());
            CreateIndex("dbo.Hospitalizations", "client_client_id");
            AddForeignKey("dbo.Hospitalizations", "client_client_id", "dbo.Clients", "client_id");
        }
    }
}

namespace DataBaseClasses.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class gospitalizaciagen : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Hospitalizations", "workPlace", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Hospitalizations", "workPlace");
        }
    }
}

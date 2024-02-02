namespace МИС__ГКБ_Большие_Кабаны_.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class passportgetinfo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Clients", "passportGetInfo", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Clients", "passportGetInfo");
        }
    }
}

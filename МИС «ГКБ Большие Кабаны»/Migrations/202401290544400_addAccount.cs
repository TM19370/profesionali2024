namespace МИС__ГКБ_Большие_Кабаны_.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addAccount : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Accounts",
                c => new
                    {
                        login = c.String(nullable: false, maxLength: 128),
                        password = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.login);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Accounts");
        }
    }
}

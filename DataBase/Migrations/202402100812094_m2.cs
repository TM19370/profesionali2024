namespace DataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class m2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Timetables", "cabinet", c => c.String());
            AlterColumn("dbo.Timetables", "startTime", c => c.DateTime());
            AlterColumn("dbo.Timetables", "endTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Timetables", "endTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Timetables", "startTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Timetables", "cabinet", c => c.String(nullable: false));
        }
    }
}

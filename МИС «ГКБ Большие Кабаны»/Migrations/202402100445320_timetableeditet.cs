namespace МИС__ГКБ_Большие_Кабаны_.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class timetableeditet : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Timetables", "startTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.Timetables", "endTime", c => c.DateTime(nullable: false));
            DropColumn("dbo.Timetables", "timeSpan");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Timetables", "timeSpan", c => c.Time(nullable: false, precision: 7));
            DropColumn("dbo.Timetables", "endTime");
            DropColumn("dbo.Timetables", "startTime");
        }
    }
}

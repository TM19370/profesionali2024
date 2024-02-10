namespace МИС__ГКБ_Большие_Кабаны_.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class m1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WeekTimetables", "weekFirstDayDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.WeekTimetables", "weekStartDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.WeekTimetables", "weekStartDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.WeekTimetables", "weekFirstDayDate");
        }
    }
}

namespace DataBaseClasses.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2902 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.WeekTimetables", "account_account_id", "dbo.Accounts");
            DropForeignKey("dbo.Timetables", "weekTimetable_weekTimeTable_id", "dbo.WeekTimetables");
            DropIndex("dbo.Timetables", new[] { "weekTimetable_weekTimeTable_id" });
            DropIndex("dbo.WeekTimetables", new[] { "account_account_id" });
            CreateTable(
                "dbo.Appointments",
                c => new
                    {
                        appointment_id = c.Int(nullable: false, identity: true),
                        client_client_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.appointment_id)
                .ForeignKey("dbo.Clients", t => t.client_client_id, cascadeDelete: true)
                .Index(t => t.client_client_id);
            
            CreateTable(
                "dbo.DayOfWeeks",
                c => new
                    {
                        dayOfWeek_id = c.Int(nullable: false, identity: true),
                        name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.dayOfWeek_id);
            
            CreateTable(
                "dbo.ScheduleElements",
                c => new
                    {
                        scheduleElement_id = c.Int(nullable: false, identity: true),
                        account_account_id = c.Int(nullable: false),
                        dayOfWeek_dayOfWeek_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.scheduleElement_id)
                .ForeignKey("dbo.Accounts", t => t.account_account_id, cascadeDelete: true)
                .ForeignKey("dbo.DayOfWeeks", t => t.dayOfWeek_dayOfWeek_id, cascadeDelete: true)
                .Index(t => t.account_account_id)
                .Index(t => t.dayOfWeek_dayOfWeek_id);
            
            CreateTable(
                "dbo.WorkTimes",
                c => new
                    {
                        workTime_id = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.workTime_id);
            
            DropTable("dbo.Timetables");
            DropTable("dbo.WeekTimetables");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.WeekTimetables",
                c => new
                    {
                        weekTimeTable_id = c.Int(nullable: false, identity: true),
                        weekFirstDayDate = c.DateTime(nullable: false),
                        account_account_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.weekTimeTable_id);
            
            CreateTable(
                "dbo.Timetables",
                c => new
                    {
                        timetable_id = c.Int(nullable: false, identity: true),
                        dayOfWeek = c.Int(nullable: false),
                        cabinet = c.String(),
                        startTime = c.DateTime(),
                        endTime = c.DateTime(),
                        weekTimetable_weekTimeTable_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.timetable_id);
            
            DropForeignKey("dbo.ScheduleElements", "dayOfWeek_dayOfWeek_id", "dbo.DayOfWeeks");
            DropForeignKey("dbo.ScheduleElements", "account_account_id", "dbo.Accounts");
            DropForeignKey("dbo.Appointments", "client_client_id", "dbo.Clients");
            DropIndex("dbo.ScheduleElements", new[] { "dayOfWeek_dayOfWeek_id" });
            DropIndex("dbo.ScheduleElements", new[] { "account_account_id" });
            DropIndex("dbo.Appointments", new[] { "client_client_id" });
            DropTable("dbo.WorkTimes");
            DropTable("dbo.ScheduleElements");
            DropTable("dbo.DayOfWeeks");
            DropTable("dbo.Appointments");
            CreateIndex("dbo.WeekTimetables", "account_account_id");
            CreateIndex("dbo.Timetables", "weekTimetable_weekTimeTable_id");
            AddForeignKey("dbo.Timetables", "weekTimetable_weekTimeTable_id", "dbo.WeekTimetables", "weekTimeTable_id", cascadeDelete: true);
            AddForeignKey("dbo.WeekTimetables", "account_account_id", "dbo.Accounts", "account_id", cascadeDelete: true);
        }
    }
}

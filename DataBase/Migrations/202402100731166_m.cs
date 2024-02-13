namespace DataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class m : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Timetables", "doctor_doctor_id", "dbo.Doctors");
            DropIndex("dbo.Timetables", new[] { "doctor_doctor_id" });
            CreateTable(
                "dbo.WeekTimetables",
                c => new
                    {
                        weekTimeTable_id = c.Int(nullable: false, identity: true),
                        weekStartDate = c.DateTime(nullable: false),
                        doctor_doctor_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.weekTimeTable_id)
                .ForeignKey("dbo.Doctors", t => t.doctor_doctor_id, cascadeDelete: true)
                .Index(t => t.doctor_doctor_id);
            
            AddColumn("dbo.Timetables", "weekTimetable_weekTimeTable_id", c => c.Int(nullable: false));
            CreateIndex("dbo.Timetables", "weekTimetable_weekTimeTable_id");
            AddForeignKey("dbo.Timetables", "weekTimetable_weekTimeTable_id", "dbo.WeekTimetables", "weekTimeTable_id", cascadeDelete: true);
            DropColumn("dbo.Timetables", "doctor_doctor_id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Timetables", "doctor_doctor_id", c => c.Int(nullable: false));
            DropForeignKey("dbo.Timetables", "weekTimetable_weekTimeTable_id", "dbo.WeekTimetables");
            DropForeignKey("dbo.WeekTimetables", "doctor_doctor_id", "dbo.Doctors");
            DropIndex("dbo.WeekTimetables", new[] { "doctor_doctor_id" });
            DropIndex("dbo.Timetables", new[] { "weekTimetable_weekTimeTable_id" });
            DropColumn("dbo.Timetables", "weekTimetable_weekTimeTable_id");
            DropTable("dbo.WeekTimetables");
            CreateIndex("dbo.Timetables", "doctor_doctor_id");
            AddForeignKey("dbo.Timetables", "doctor_doctor_id", "dbo.Doctors", "doctor_id", cascadeDelete: true);
        }
    }
}

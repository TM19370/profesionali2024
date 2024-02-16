namespace DataBaseClasses.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _16021 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Prescriptions", "appointmentInfo_appointmentInfo_id", "dbo.AppointmentInfoes");
            DropIndex("dbo.Prescriptions", new[] { "appointmentInfo_appointmentInfo_id" });
            AlterColumn("dbo.Prescriptions", "appointmentInfo_appointmentInfo_id", c => c.Int());
            CreateIndex("dbo.Prescriptions", "appointmentInfo_appointmentInfo_id");
            AddForeignKey("dbo.Prescriptions", "appointmentInfo_appointmentInfo_id", "dbo.AppointmentInfoes", "appointmentInfo_id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Prescriptions", "appointmentInfo_appointmentInfo_id", "dbo.AppointmentInfoes");
            DropIndex("dbo.Prescriptions", new[] { "appointmentInfo_appointmentInfo_id" });
            AlterColumn("dbo.Prescriptions", "appointmentInfo_appointmentInfo_id", c => c.Int(nullable: false));
            CreateIndex("dbo.Prescriptions", "appointmentInfo_appointmentInfo_id");
            AddForeignKey("dbo.Prescriptions", "appointmentInfo_appointmentInfo_id", "dbo.AppointmentInfoes", "appointmentInfo_id", cascadeDelete: true);
        }
    }
}

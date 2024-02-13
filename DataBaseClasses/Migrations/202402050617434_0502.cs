namespace DataBaseClasses.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _0502 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AppointmentInfoes",
                c => new
                    {
                        appointmentInfo_id = c.Int(nullable: false, identity: true),
                        anamnesis = c.String(nullable: false),
                        symptoms = c.String(nullable: false),
                        diagnosis = c.String(nullable: false),
                        recommendations = c.String(nullable: false),
                        client_client_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.appointmentInfo_id)
                .ForeignKey("dbo.Clients", t => t.client_client_id, cascadeDelete: true)
                .Index(t => t.client_client_id);
            
            CreateTable(
                "dbo.Medicaments",
                c => new
                    {
                        medicamentName = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.medicamentName);
            
            CreateTable(
                "dbo.Prescriptions",
                c => new
                    {
                        prescription_id = c.Int(nullable: false, identity: true),
                        dose = c.Double(nullable: false),
                        format = c.String(nullable: false),
                        appointmentInfo_appointmentInfo_id = c.Int(nullable: false),
                        medicament_medicamentName = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.prescription_id)
                .ForeignKey("dbo.AppointmentInfoes", t => t.appointmentInfo_appointmentInfo_id, cascadeDelete: true)
                .ForeignKey("dbo.Medicaments", t => t.medicament_medicamentName, cascadeDelete: true)
                .Index(t => t.appointmentInfo_appointmentInfo_id)
                .Index(t => t.medicament_medicamentName);
            
            CreateTable(
                "dbo.Timetables",
                c => new
                    {
                        timetable_id = c.Int(nullable: false, identity: true),
                        cabinet = c.String(nullable: false),
                        dayOfWeek = c.Int(nullable: false),
                        timeSpan = c.Time(nullable: false, precision: 7),
                        doctor_doctor_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.timetable_id)
                .ForeignKey("dbo.Doctors", t => t.doctor_doctor_id, cascadeDelete: true)
                .Index(t => t.doctor_doctor_id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Timetables", "doctor_doctor_id", "dbo.Doctors");
            DropForeignKey("dbo.Prescriptions", "medicament_medicamentName", "dbo.Medicaments");
            DropForeignKey("dbo.Prescriptions", "appointmentInfo_appointmentInfo_id", "dbo.AppointmentInfoes");
            DropForeignKey("dbo.AppointmentInfoes", "client_client_id", "dbo.Clients");
            DropIndex("dbo.Timetables", new[] { "doctor_doctor_id" });
            DropIndex("dbo.Prescriptions", new[] { "medicament_medicamentName" });
            DropIndex("dbo.Prescriptions", new[] { "appointmentInfo_appointmentInfo_id" });
            DropIndex("dbo.AppointmentInfoes", new[] { "client_client_id" });
            DropTable("dbo.Timetables");
            DropTable("dbo.Prescriptions");
            DropTable("dbo.Medicaments");
            DropTable("dbo.AppointmentInfoes");
        }
    }
}

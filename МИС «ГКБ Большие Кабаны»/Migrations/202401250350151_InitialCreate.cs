namespace МИС__ГКБ_Большие_Кабаны_.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Clients",
                c => new
                    {
                        client_id = c.Int(nullable: false, identity: true),
                        photoPath = c.String(nullable: false),
                        firstName = c.String(nullable: false),
                        secondName = c.String(nullable: false),
                        lastName = c.String(nullable: false),
                        passportNumberAndSeries = c.String(nullable: false),
                        birthDate = c.DateTime(nullable: false),
                        address = c.String(nullable: false),
                        phoneNumder = c.String(nullable: false),
                        email = c.String(nullable: false),
                        medicalCardNumber = c.Int(nullable: false),
                        getMedicalCardDate = c.DateTime(nullable: false),
                        lastVisitDate = c.DateTime(nullable: false),
                        nextVisitDate = c.DateTime(nullable: false),
                        insurancePolicyNumber = c.Int(nullable: false),
                        insurancePolicyEndDate = c.DateTime(nullable: false),
                        gender_genderName = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.client_id)
                .ForeignKey("dbo.Genders", t => t.gender_genderName, cascadeDelete: true)
                .Index(t => t.gender_genderName);
            
            CreateTable(
                "dbo.Genders",
                c => new
                    {
                        genderName = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.genderName);
            
            CreateTable(
                "dbo.Diagnosis",
                c => new
                    {
                        diagnosisName = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.diagnosisName);
            
            CreateTable(
                "dbo.Doctors",
                c => new
                    {
                        doctor_id = c.Int(nullable: false, identity: true),
                        firstName = c.String(nullable: false),
                        secondName = c.String(nullable: false),
                        lastName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.doctor_id);
            
            CreateTable(
                "dbo.Measures",
                c => new
                    {
                        measure_id = c.Int(nullable: false, identity: true),
                        measureDate = c.DateTime(nullable: false),
                        measureName = c.String(nullable: false),
                        measureResault = c.String(nullable: false),
                        recommendations = c.String(nullable: false),
                        client_client_id = c.Int(nullable: false),
                        doctor_doctor_id = c.Int(nullable: false),
                        measureType_measureTypeName = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.measure_id)
                .ForeignKey("dbo.Clients", t => t.client_client_id, cascadeDelete: true)
                .ForeignKey("dbo.Doctors", t => t.doctor_doctor_id, cascadeDelete: true)
                .ForeignKey("dbo.MeasureTypes", t => t.measureType_measureTypeName, cascadeDelete: true)
                .Index(t => t.client_client_id)
                .Index(t => t.doctor_doctor_id)
                .Index(t => t.measureType_measureTypeName);
            
            CreateTable(
                "dbo.MeasureTypes",
                c => new
                    {
                        measureTypeName = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.measureTypeName);
            
            CreateTable(
                "dbo.MedicalHistories",
                c => new
                    {
                        medicalHistory_id = c.Int(nullable: false, identity: true),
                        client_client_id = c.Int(nullable: false),
                        diagnosis_diagnosisName = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.medicalHistory_id)
                .ForeignKey("dbo.Clients", t => t.client_client_id, cascadeDelete: true)
                .ForeignKey("dbo.Diagnosis", t => t.diagnosis_diagnosisName, cascadeDelete: true)
                .Index(t => t.client_client_id)
                .Index(t => t.diagnosis_diagnosisName);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MedicalHistories", "diagnosis_diagnosisName", "dbo.Diagnosis");
            DropForeignKey("dbo.MedicalHistories", "client_client_id", "dbo.Clients");
            DropForeignKey("dbo.Measures", "measureType_measureTypeName", "dbo.MeasureTypes");
            DropForeignKey("dbo.Measures", "doctor_doctor_id", "dbo.Doctors");
            DropForeignKey("dbo.Measures", "client_client_id", "dbo.Clients");
            DropForeignKey("dbo.Clients", "gender_genderName", "dbo.Genders");
            DropIndex("dbo.MedicalHistories", new[] { "diagnosis_diagnosisName" });
            DropIndex("dbo.MedicalHistories", new[] { "client_client_id" });
            DropIndex("dbo.Measures", new[] { "measureType_measureTypeName" });
            DropIndex("dbo.Measures", new[] { "doctor_doctor_id" });
            DropIndex("dbo.Measures", new[] { "client_client_id" });
            DropIndex("dbo.Clients", new[] { "gender_genderName" });
            DropTable("dbo.MedicalHistories");
            DropTable("dbo.MeasureTypes");
            DropTable("dbo.Measures");
            DropTable("dbo.Doctors");
            DropTable("dbo.Diagnosis");
            DropTable("dbo.Genders");
            DropTable("dbo.Clients");
        }
    }
}

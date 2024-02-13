namespace DataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class gospitalizacia : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Hospitalizations",
                c => new
                    {
                        hospitalization_id = c.Int(nullable: false, identity: true),
                        firstName = c.String(nullable: false),
                        secondName = c.String(nullable: false),
                        lastName = c.String(nullable: false),
                        passportNumberAndSeries = c.String(nullable: false),
                        passportGetInfo = c.String(),
                        birthDate = c.DateTime(nullable: false),
                        address = c.String(nullable: false),
                        phoneNumder = c.String(nullable: false),
                        email = c.String(nullable: false),
                        medicalCardNumber = c.Int(nullable: false),
                        getMedicalCardDate = c.DateTime(nullable: false),
                        lastVisitDate = c.DateTime(nullable: false),
                        nextVisitDate = c.DateTime(nullable: false),
                        insurancePolicyNumber = c.String(nullable: false),
                        insurancePolicyEndDate = c.DateTime(nullable: false),
                        insuranceCompany = c.String(),
                        diagnosis = c.String(),
                        hospitalizationPurpose = c.String(),
                        hospitalizationDepartment = c.String(),
                        hospitalizationCondition = c.String(),
                        hospitalizationStartDate = c.DateTime(nullable: false),
                        hospitalizationEndDate = c.DateTime(nullable: false),
                        hospitalizationAddInfo = c.String(),
                        hospitalizationCancelInfo = c.String(),
                        gender_genderName = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.hospitalization_id)
                .ForeignKey("dbo.Genders", t => t.gender_genderName, cascadeDelete: true)
                .Index(t => t.gender_genderName);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Hospitalizations", "gender_genderName", "dbo.Genders");
            DropIndex("dbo.Hospitalizations", new[] { "gender_genderName" });
            DropTable("dbo.Hospitalizations");
        }
    }
}

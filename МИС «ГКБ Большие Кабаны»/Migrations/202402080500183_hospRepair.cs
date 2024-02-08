namespace МИС__ГКБ_Большие_Кабаны_.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class hospRepair : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Hospitalizations", "gender_gender_id", "dbo.Genders");
            DropIndex("dbo.Hospitalizations", new[] { "gender_gender_id" });
            AddColumn("dbo.Clients", "workPlace", c => c.String());
            AddColumn("dbo.Clients", "insuranceCompany", c => c.String());
            AddColumn("dbo.Hospitalizations", "client_client_id", c => c.Int());
            CreateIndex("dbo.Hospitalizations", "client_client_id");
            AddForeignKey("dbo.Hospitalizations", "client_client_id", "dbo.Clients", "client_id");
            DropColumn("dbo.Hospitalizations", "firstName");
            DropColumn("dbo.Hospitalizations", "secondName");
            DropColumn("dbo.Hospitalizations", "lastName");
            DropColumn("dbo.Hospitalizations", "passportNumberAndSeries");
            DropColumn("dbo.Hospitalizations", "passportGetInfo");
            DropColumn("dbo.Hospitalizations", "birthDate");
            DropColumn("dbo.Hospitalizations", "workPlace");
            DropColumn("dbo.Hospitalizations", "address");
            DropColumn("dbo.Hospitalizations", "phoneNumder");
            DropColumn("dbo.Hospitalizations", "email");
            DropColumn("dbo.Hospitalizations", "medicalCardNumber");
            DropColumn("dbo.Hospitalizations", "getMedicalCardDate");
            DropColumn("dbo.Hospitalizations", "lastVisitDate");
            DropColumn("dbo.Hospitalizations", "nextVisitDate");
            DropColumn("dbo.Hospitalizations", "insurancePolicyNumber");
            DropColumn("dbo.Hospitalizations", "insurancePolicyEndDate");
            DropColumn("dbo.Hospitalizations", "gender_gender_id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Hospitalizations", "gender_gender_id", c => c.Int(nullable: false));
            AddColumn("dbo.Hospitalizations", "insurancePolicyEndDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Hospitalizations", "insurancePolicyNumber", c => c.String(nullable: false));
            AddColumn("dbo.Hospitalizations", "nextVisitDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Hospitalizations", "lastVisitDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Hospitalizations", "getMedicalCardDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Hospitalizations", "medicalCardNumber", c => c.Int(nullable: false));
            AddColumn("dbo.Hospitalizations", "email", c => c.String(nullable: false));
            AddColumn("dbo.Hospitalizations", "phoneNumder", c => c.String(nullable: false));
            AddColumn("dbo.Hospitalizations", "address", c => c.String(nullable: false));
            AddColumn("dbo.Hospitalizations", "workPlace", c => c.String(nullable: false));
            AddColumn("dbo.Hospitalizations", "birthDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Hospitalizations", "passportGetInfo", c => c.String());
            AddColumn("dbo.Hospitalizations", "passportNumberAndSeries", c => c.String(nullable: false));
            AddColumn("dbo.Hospitalizations", "lastName", c => c.String(nullable: false));
            AddColumn("dbo.Hospitalizations", "secondName", c => c.String(nullable: false));
            AddColumn("dbo.Hospitalizations", "firstName", c => c.String(nullable: false));
            DropForeignKey("dbo.Hospitalizations", "client_client_id", "dbo.Clients");
            DropIndex("dbo.Hospitalizations", new[] { "client_client_id" });
            DropColumn("dbo.Hospitalizations", "client_client_id");
            DropColumn("dbo.Clients", "insuranceCompany");
            DropColumn("dbo.Clients", "workPlace");
            CreateIndex("dbo.Hospitalizations", "gender_gender_id");
            AddForeignKey("dbo.Hospitalizations", "gender_gender_id", "dbo.Genders", "gender_id", cascadeDelete: true);
        }
    }
}

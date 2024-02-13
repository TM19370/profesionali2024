namespace DataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addId : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Clients", "gender_genderName", "dbo.Genders");
            DropForeignKey("dbo.Hospitalizations", "gender_genderName", "dbo.Genders");
            DropForeignKey("dbo.MedicalHistories", "diagnosis_diagnosisName", "dbo.Diagnosis");
            DropForeignKey("dbo.Measures", "measureType_measureTypeName", "dbo.MeasureTypes");
            DropForeignKey("dbo.Prescriptions", "medicament_medicamentName", "dbo.Medicaments");
            DropIndex("dbo.Clients", new[] { "gender_genderName" });
            DropIndex("dbo.Hospitalizations", new[] { "gender_genderName" });
            DropIndex("dbo.Measures", new[] { "measureType_measureTypeName" });
            DropIndex("dbo.MedicalHistories", new[] { "diagnosis_diagnosisName" });
            DropIndex("dbo.Prescriptions", new[] { "medicament_medicamentName" });
            RenameColumn(table: "dbo.Clients", name: "gender_genderName", newName: "gender_gender_id");
            RenameColumn(table: "dbo.Hospitalizations", name: "gender_genderName", newName: "gender_gender_id");
            RenameColumn(table: "dbo.Measures", name: "measureType_measureTypeName", newName: "measureType_measureType_id");
            RenameColumn(table: "dbo.MedicalHistories", name: "diagnosis_diagnosisName", newName: "diagnosis_diagnosis_id");
            RenameColumn(table: "dbo.Prescriptions", name: "medicament_medicamentName", newName: "medicament_medicament_id");
            DropPrimaryKey("dbo.Accounts");
            DropPrimaryKey("dbo.Genders");
            DropPrimaryKey("dbo.Diagnosis");
            DropPrimaryKey("dbo.MeasureTypes");
            DropPrimaryKey("dbo.Medicaments");
            AddColumn("dbo.Accounts", "account_id", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.Genders", "gender_id", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.Diagnosis", "diagnosis_id", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.MeasureTypes", "measureType_id", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.Medicaments", "medicament_id", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.Accounts", "login", c => c.String(nullable: false));
            AlterColumn("dbo.Clients", "gender_gender_id", c => c.Int(nullable: false));
            AlterColumn("dbo.Genders", "genderName", c => c.String(nullable: false));
            AlterColumn("dbo.Diagnosis", "diagnosisName", c => c.String(nullable: false));
            AlterColumn("dbo.Hospitalizations", "gender_gender_id", c => c.Int(nullable: false));
            AlterColumn("dbo.Measures", "measureType_measureType_id", c => c.Int(nullable: false));
            AlterColumn("dbo.MeasureTypes", "measureTypeName", c => c.String(nullable: false));
            AlterColumn("dbo.MedicalHistories", "diagnosis_diagnosis_id", c => c.Int(nullable: false));
            AlterColumn("dbo.Medicaments", "medicamentName", c => c.String(nullable: false));
            AlterColumn("dbo.Prescriptions", "medicament_medicament_id", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.Accounts", "account_id");
            AddPrimaryKey("dbo.Genders", "gender_id");
            AddPrimaryKey("dbo.Diagnosis", "diagnosis_id");
            AddPrimaryKey("dbo.MeasureTypes", "measureType_id");
            AddPrimaryKey("dbo.Medicaments", "medicament_id");
            CreateIndex("dbo.Clients", "gender_gender_id");
            CreateIndex("dbo.Hospitalizations", "gender_gender_id");
            CreateIndex("dbo.Measures", "measureType_measureType_id");
            CreateIndex("dbo.MedicalHistories", "diagnosis_diagnosis_id");
            CreateIndex("dbo.Prescriptions", "medicament_medicament_id");
            AddForeignKey("dbo.Clients", "gender_gender_id", "dbo.Genders", "gender_id", cascadeDelete: true);
            AddForeignKey("dbo.Hospitalizations", "gender_gender_id", "dbo.Genders", "gender_id", cascadeDelete: true);
            AddForeignKey("dbo.MedicalHistories", "diagnosis_diagnosis_id", "dbo.Diagnosis", "diagnosis_id", cascadeDelete: true);
            AddForeignKey("dbo.Measures", "measureType_measureType_id", "dbo.MeasureTypes", "measureType_id", cascadeDelete: true);
            AddForeignKey("dbo.Prescriptions", "medicament_medicament_id", "dbo.Medicaments", "medicament_id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Prescriptions", "medicament_medicament_id", "dbo.Medicaments");
            DropForeignKey("dbo.Measures", "measureType_measureType_id", "dbo.MeasureTypes");
            DropForeignKey("dbo.MedicalHistories", "diagnosis_diagnosis_id", "dbo.Diagnosis");
            DropForeignKey("dbo.Hospitalizations", "gender_gender_id", "dbo.Genders");
            DropForeignKey("dbo.Clients", "gender_gender_id", "dbo.Genders");
            DropIndex("dbo.Prescriptions", new[] { "medicament_medicament_id" });
            DropIndex("dbo.MedicalHistories", new[] { "diagnosis_diagnosis_id" });
            DropIndex("dbo.Measures", new[] { "measureType_measureType_id" });
            DropIndex("dbo.Hospitalizations", new[] { "gender_gender_id" });
            DropIndex("dbo.Clients", new[] { "gender_gender_id" });
            DropPrimaryKey("dbo.Medicaments");
            DropPrimaryKey("dbo.MeasureTypes");
            DropPrimaryKey("dbo.Diagnosis");
            DropPrimaryKey("dbo.Genders");
            DropPrimaryKey("dbo.Accounts");
            AlterColumn("dbo.Prescriptions", "medicament_medicament_id", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Medicaments", "medicamentName", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.MedicalHistories", "diagnosis_diagnosis_id", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.MeasureTypes", "measureTypeName", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Measures", "measureType_measureType_id", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Hospitalizations", "gender_gender_id", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Diagnosis", "diagnosisName", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Genders", "genderName", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Clients", "gender_gender_id", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Accounts", "login", c => c.String(nullable: false, maxLength: 128));
            DropColumn("dbo.Medicaments", "medicament_id");
            DropColumn("dbo.MeasureTypes", "measureType_id");
            DropColumn("dbo.Diagnosis", "diagnosis_id");
            DropColumn("dbo.Genders", "gender_id");
            DropColumn("dbo.Accounts", "account_id");
            AddPrimaryKey("dbo.Medicaments", "medicamentName");
            AddPrimaryKey("dbo.MeasureTypes", "measureTypeName");
            AddPrimaryKey("dbo.Diagnosis", "diagnosisName");
            AddPrimaryKey("dbo.Genders", "genderName");
            AddPrimaryKey("dbo.Accounts", "login");
            RenameColumn(table: "dbo.Prescriptions", name: "medicament_medicament_id", newName: "medicament_medicamentName");
            RenameColumn(table: "dbo.MedicalHistories", name: "diagnosis_diagnosis_id", newName: "diagnosis_diagnosisName");
            RenameColumn(table: "dbo.Measures", name: "measureType_measureType_id", newName: "measureType_measureTypeName");
            RenameColumn(table: "dbo.Hospitalizations", name: "gender_gender_id", newName: "gender_genderName");
            RenameColumn(table: "dbo.Clients", name: "gender_gender_id", newName: "gender_genderName");
            CreateIndex("dbo.Prescriptions", "medicament_medicamentName");
            CreateIndex("dbo.MedicalHistories", "diagnosis_diagnosisName");
            CreateIndex("dbo.Measures", "measureType_measureTypeName");
            CreateIndex("dbo.Hospitalizations", "gender_genderName");
            CreateIndex("dbo.Clients", "gender_genderName");
            AddForeignKey("dbo.Prescriptions", "medicament_medicamentName", "dbo.Medicaments", "medicamentName", cascadeDelete: true);
            AddForeignKey("dbo.Measures", "measureType_measureTypeName", "dbo.MeasureTypes", "measureTypeName", cascadeDelete: true);
            AddForeignKey("dbo.MedicalHistories", "diagnosis_diagnosisName", "dbo.Diagnosis", "diagnosisName", cascadeDelete: true);
            AddForeignKey("dbo.Hospitalizations", "gender_genderName", "dbo.Genders", "genderName", cascadeDelete: true);
            AddForeignKey("dbo.Clients", "gender_genderName", "dbo.Genders", "genderName", cascadeDelete: true);
        }
    }
}

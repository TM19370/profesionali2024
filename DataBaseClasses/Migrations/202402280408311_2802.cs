namespace DataBaseClasses.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2802 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Measures", "doctor_doctor_id", "dbo.Doctors");
            DropForeignKey("dbo.WeekTimetables", "doctor_doctor_id", "dbo.Doctors");
            DropIndex("dbo.Measures", new[] { "doctor_doctor_id" });
            DropIndex("dbo.WeekTimetables", new[] { "doctor_doctor_id" });
            CreateTable(
                "dbo.AccountTypes",
                c => new
                    {
                        accountType_id = c.Int(nullable: false, identity: true),
                        typeName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.accountType_id);
            
            CreateTable(
                "dbo.FullNames",
                c => new
                    {
                        fullName_id = c.Int(nullable: false, identity: true),
                        firstName = c.String(nullable: false),
                        secondName = c.String(nullable: false),
                        lastName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.fullName_id);
            
            AddColumn("dbo.Accounts", "accountType_accountType_id", c => c.Int(nullable: false));
            AddColumn("dbo.Clients", "fullName_fullName_id", c => c.Int(nullable: false));
            AddColumn("dbo.Measures", "account_account_id", c => c.Int(nullable: false));
            AddColumn("dbo.WeekTimetables", "account_account_id", c => c.Int(nullable: false));
            CreateIndex("dbo.Accounts", "accountType_accountType_id");
            CreateIndex("dbo.Clients", "fullName_fullName_id");
            CreateIndex("dbo.Measures", "account_account_id");
            CreateIndex("dbo.WeekTimetables", "account_account_id");
            AddForeignKey("dbo.Accounts", "accountType_accountType_id", "dbo.AccountTypes", "accountType_id", cascadeDelete: true);
            AddForeignKey("dbo.Clients", "fullName_fullName_id", "dbo.FullNames", "fullName_id", cascadeDelete: true);
            AddForeignKey("dbo.Measures", "account_account_id", "dbo.Accounts", "account_id", cascadeDelete: true);
            AddForeignKey("dbo.WeekTimetables", "account_account_id", "dbo.Accounts", "account_id", cascadeDelete: true);
            DropColumn("dbo.Clients", "firstName");
            DropColumn("dbo.Clients", "secondName");
            DropColumn("dbo.Clients", "lastName");
            DropColumn("dbo.Measures", "doctor_doctor_id");
            DropColumn("dbo.WeekTimetables", "doctor_doctor_id");
            DropTable("dbo.Doctors");
        }
        
        public override void Down()
        {
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
            
            AddColumn("dbo.WeekTimetables", "doctor_doctor_id", c => c.Int(nullable: false));
            AddColumn("dbo.Measures", "doctor_doctor_id", c => c.Int(nullable: false));
            AddColumn("dbo.Clients", "lastName", c => c.String(nullable: false));
            AddColumn("dbo.Clients", "secondName", c => c.String(nullable: false));
            AddColumn("dbo.Clients", "firstName", c => c.String(nullable: false));
            DropForeignKey("dbo.WeekTimetables", "account_account_id", "dbo.Accounts");
            DropForeignKey("dbo.Measures", "account_account_id", "dbo.Accounts");
            DropForeignKey("dbo.Clients", "fullName_fullName_id", "dbo.FullNames");
            DropForeignKey("dbo.Accounts", "accountType_accountType_id", "dbo.AccountTypes");
            DropIndex("dbo.WeekTimetables", new[] { "account_account_id" });
            DropIndex("dbo.Measures", new[] { "account_account_id" });
            DropIndex("dbo.Clients", new[] { "fullName_fullName_id" });
            DropIndex("dbo.Accounts", new[] { "accountType_accountType_id" });
            DropColumn("dbo.WeekTimetables", "account_account_id");
            DropColumn("dbo.Measures", "account_account_id");
            DropColumn("dbo.Clients", "fullName_fullName_id");
            DropColumn("dbo.Accounts", "accountType_accountType_id");
            DropTable("dbo.FullNames");
            DropTable("dbo.AccountTypes");
            CreateIndex("dbo.WeekTimetables", "doctor_doctor_id");
            CreateIndex("dbo.Measures", "doctor_doctor_id");
            AddForeignKey("dbo.WeekTimetables", "doctor_doctor_id", "dbo.Doctors", "doctor_id", cascadeDelete: true);
            AddForeignKey("dbo.Measures", "doctor_doctor_id", "dbo.Doctors", "doctor_id", cascadeDelete: true);
        }
    }
}

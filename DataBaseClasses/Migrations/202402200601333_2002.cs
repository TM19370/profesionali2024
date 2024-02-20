namespace DataBaseClasses.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2002 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AppointmentInfoes", "audioMessageFileName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AppointmentInfoes", "audioMessageFileName");
        }
    }
}

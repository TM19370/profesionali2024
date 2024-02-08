namespace МИС__ГКБ_Большие_Кабаны_.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class hospDateFix : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Hospitalizations", "hospitalizationStartDate", c => c.DateTime());
            AlterColumn("dbo.Hospitalizations", "hospitalizationEndDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Hospitalizations", "hospitalizationEndDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Hospitalizations", "hospitalizationStartDate", c => c.DateTime(nullable: false));
        }
    }
}

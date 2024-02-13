﻿namespace DataBaseClasses.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Clients", "FullName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Clients", "FullName");
        }
    }
}

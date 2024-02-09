namespace МИС__ГКБ_Большие_Кабаны_.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addwarehouses : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.WarehouseForMedicaments",
                c => new
                    {
                        warehouseForMedicament_id = c.Int(nullable: false, identity: true),
                        medicamentCount = c.Int(nullable: false),
                        medicament_medicament_id = c.Int(nullable: false),
                        warehouse_warehouse_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.warehouseForMedicament_id)
                .ForeignKey("dbo.Medicaments", t => t.medicament_medicament_id, cascadeDelete: true)
                .ForeignKey("dbo.Warehouses", t => t.warehouse_warehouse_id, cascadeDelete: true)
                .Index(t => t.medicament_medicament_id)
                .Index(t => t.warehouse_warehouse_id);
            
            CreateTable(
                "dbo.Warehouses",
                c => new
                    {
                        warehouse_id = c.Int(nullable: false, identity: true),
                        warehouseName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.warehouse_id);
            
            DropColumn("dbo.Hospitalizations", "insuranceCompany");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Hospitalizations", "insuranceCompany", c => c.String());
            DropForeignKey("dbo.WarehouseForMedicaments", "warehouse_warehouse_id", "dbo.Warehouses");
            DropForeignKey("dbo.WarehouseForMedicaments", "medicament_medicament_id", "dbo.Medicaments");
            DropIndex("dbo.WarehouseForMedicaments", new[] { "warehouse_warehouse_id" });
            DropIndex("dbo.WarehouseForMedicaments", new[] { "medicament_medicament_id" });
            DropTable("dbo.Warehouses");
            DropTable("dbo.WarehouseForMedicaments");
        }
    }
}

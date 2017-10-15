namespace Cashed.DataAccess.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Incomes : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.IncomeItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IncomeTypeId = c.Int(nullable: false),
                        DateTime = c.DateTime(nullable: false),
                        Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.IncomeTypes", t => t.IncomeTypeId, cascadeDelete: true)
                .Index(t => t.IncomeTypeId);
            
            CreateTable(
                "dbo.IncomeTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.IncomeItems", "IncomeTypeId", "dbo.IncomeTypes");
            DropIndex("dbo.IncomeItems", new[] { "IncomeTypeId" });
            DropTable("dbo.IncomeTypes");
            DropTable("dbo.IncomeItems");
        }
    }
}

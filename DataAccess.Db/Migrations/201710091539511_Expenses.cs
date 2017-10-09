namespace Cashed.DataAccess.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Expenses : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ExpenseBills",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateTime = c.DateTime(nullable: false),
                        SumPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ExpenseItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CategoryId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Quantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Comment = c.String(),
                        ExpenseBill_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Products", t => t.ProductId)
                .ForeignKey("dbo.ExpenseBills", t => t.ExpenseBill_Id)
                .Index(t => t.ProductId)
                .Index(t => t.ExpenseBill_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ExpenseItems", "ExpenseBill_Id", "dbo.ExpenseBills");
            DropForeignKey("dbo.ExpenseItems", "ProductId", "dbo.Products");
            DropIndex("dbo.ExpenseItems", new[] { "ExpenseBill_Id" });
            DropIndex("dbo.ExpenseItems", new[] { "ProductId" });
            DropTable("dbo.ExpenseItems");
            DropTable("dbo.ExpenseBills");
        }
    }
}

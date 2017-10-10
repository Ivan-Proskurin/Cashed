namespace Cashed.DataAccess.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Expsense_Upgrade : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ExpenseItems", "ExpenseBill_Id", "dbo.ExpenseBills");
            DropIndex("dbo.ExpenseItems", new[] { "ExpenseBill_Id" });
            RenameColumn(table: "dbo.ExpenseItems", name: "ExpenseBill_Id", newName: "BillId");
            AlterColumn("dbo.ExpenseItems", "BillId", c => c.Int(nullable: false));
            CreateIndex("dbo.ExpenseItems", "BillId");
            AddForeignKey("dbo.ExpenseItems", "BillId", "dbo.ExpenseBills", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ExpenseItems", "BillId", "dbo.ExpenseBills");
            DropIndex("dbo.ExpenseItems", new[] { "BillId" });
            AlterColumn("dbo.ExpenseItems", "BillId", c => c.Int());
            RenameColumn(table: "dbo.ExpenseItems", name: "BillId", newName: "ExpenseBill_Id");
            CreateIndex("dbo.ExpenseItems", "ExpenseBill_Id");
            AddForeignKey("dbo.ExpenseItems", "ExpenseBill_Id", "dbo.ExpenseBills", "Id");
        }
    }
}

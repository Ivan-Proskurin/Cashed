namespace Cashed.DataAccess.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AccountIsMandatory : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ExpenseBills", "AccountId", "dbo.Accounts");
            DropForeignKey("dbo.IncomeItems", "AccountId", "dbo.Accounts");
            DropIndex("dbo.ExpenseBills", new[] { "AccountId" });
            DropIndex("dbo.IncomeItems", new[] { "AccountId" });
            AlterColumn("dbo.ExpenseBills", "AccountId", c => c.Int(nullable: false));
            AlterColumn("dbo.IncomeItems", "AccountId", c => c.Int(nullable: false));
            CreateIndex("dbo.ExpenseBills", "AccountId");
            CreateIndex("dbo.IncomeItems", "AccountId");
            AddForeignKey("dbo.ExpenseBills", "AccountId", "dbo.Accounts", "Id", cascadeDelete: true);
            AddForeignKey("dbo.IncomeItems", "AccountId", "dbo.Accounts", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.IncomeItems", "AccountId", "dbo.Accounts");
            DropForeignKey("dbo.ExpenseBills", "AccountId", "dbo.Accounts");
            DropIndex("dbo.IncomeItems", new[] { "AccountId" });
            DropIndex("dbo.ExpenseBills", new[] { "AccountId" });
            AlterColumn("dbo.IncomeItems", "AccountId", c => c.Int());
            AlterColumn("dbo.ExpenseBills", "AccountId", c => c.Int());
            CreateIndex("dbo.IncomeItems", "AccountId");
            CreateIndex("dbo.ExpenseBills", "AccountId");
            AddForeignKey("dbo.IncomeItems", "AccountId", "dbo.Accounts", "Id");
            AddForeignKey("dbo.ExpenseBills", "AccountId", "dbo.Accounts", "Id");
        }
    }
}

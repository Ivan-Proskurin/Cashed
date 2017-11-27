namespace Cashed.DataAccess.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Accounts : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Accounts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Balance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.ExpenseBills", "AccountId", c => c.Int());
            AddColumn("dbo.IncomeItems", "AccountId", c => c.Int());
            CreateIndex("dbo.ExpenseBills", "AccountId");
            CreateIndex("dbo.IncomeItems", "AccountId");
            AddForeignKey("dbo.ExpenseBills", "AccountId", "dbo.Accounts", "Id");
            AddForeignKey("dbo.IncomeItems", "AccountId", "dbo.Accounts", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.IncomeItems", "AccountId", "dbo.Accounts");
            DropForeignKey("dbo.ExpenseBills", "AccountId", "dbo.Accounts");
            DropIndex("dbo.IncomeItems", new[] { "AccountId" });
            DropIndex("dbo.ExpenseBills", new[] { "AccountId" });
            DropColumn("dbo.IncomeItems", "AccountId");
            DropColumn("dbo.ExpenseBills", "AccountId");
            DropTable("dbo.Accounts");
        }
    }
}

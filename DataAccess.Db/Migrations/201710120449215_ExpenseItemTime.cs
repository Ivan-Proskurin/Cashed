namespace Cashed.DataAccess.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ExpenseItemTime : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ExpenseItems", "DateTime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ExpenseItems", "DateTime");
        }
    }
}

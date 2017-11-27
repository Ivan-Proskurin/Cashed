using Cashed.DataAccess.Model.Expenses;
using System.Data.Entity;
using Cashed.DataAccess.Model.Base;
using Cashed.DataAccess.Model.Incomes;

namespace Cashed.DataAccess.Db
{
    public class CashedDbContext : DbContext
    {
        public CashedDbContext() : base("CashedDb")
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ExpenseItem> ExpenseItems { get; set; }
        public DbSet<ExpenseBill> ExpenseBills { get; set; }
        public DbSet<IncomeType> IncomeTypes { get; set; }
        public DbSet<IncomeItem> IncomeItems { get; set; }
        public DbSet<Account> Accounts { get; set; }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ExpenseItem>()
                .HasRequired(e => e.Product)
                .WithMany()
                .WillCascadeOnDelete(false);
        }
    }
}

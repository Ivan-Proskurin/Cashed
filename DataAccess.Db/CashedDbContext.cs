using Cashed.DataAccess.Model;
using Cashed.DataAccess.Model.Expenses;
using System.Data.Entity;

namespace Cashed.DataAccess.Db
{
    public class CashedDbContext : DbContext
    {
        public CashedDbContext() : base ("CashedDb")
        { 
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ExpenseItem> ExpenseItems { get; set; }
        public DbSet<ExpenseBill> ExpenseBills { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            modelBuilder.Entity<ExpenseItem>()
                .HasRequired(e => e.Product)
                .WithMany()
                .WillCascadeOnDelete(false);

            //modelBuilder.Entity<ExpenseItem>()
            //    .HasRequired(e => e.Category)
            //    .WithMany()
            //    .WillCascadeOnDelete(false);
        }
    }
}

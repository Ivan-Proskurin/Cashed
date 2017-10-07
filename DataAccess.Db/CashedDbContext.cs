using Cashed.DataAccess.Model;
using System.Data.Entity;

namespace Cashed.DataAccess.Db
{
    public class CashedDbContext : DbContext
    {
        public CashedDbContext() : base ("CashedProjectDb")
        {

        }

        public DbSet<Category> Caategories { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}

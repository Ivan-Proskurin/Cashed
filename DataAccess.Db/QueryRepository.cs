using Cashed.DataAccess.Contract;
using System.Data.Entity;
using System.Linq;

namespace Cashed.DataAccess.Db
{
    public class QueryRepository<T> : IQueryRepository<T> where T : class
    {
        private DbSet<T> _dbSet;

        public QueryRepository(DbSet<T> dbSet)
        {
            _dbSet = dbSet;
        }

        public IQueryable<T> Query => _dbSet;
    }
}

using Cashed.DataAccess.Contract;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Cashed.DataAccess.Contract.Base;

namespace Cashed.DataAccess.Db
{
    public class QueryRepository<T> : IQueryRepository<T> where T : class, IHasId
    {
        private readonly DbSet<T> _dbSet;

        public QueryRepository(DbSet<T> dbSet)
        {
            _dbSet = dbSet;
        }

        public IQueryable<T> Query => _dbSet;

        public async Task<T> GetById(int id)
        {
            return await Query.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}

using Cashed.DataAccess.Contract;
using Cashed.DataAccess.Model.Basic;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Cashed.DataAccess.Db
{
    public class QueryRepository<T> : IQueryRepository<T> where T : class, IHasName, IHasId
    {
        private DbSet<T> _dbSet;

        public QueryRepository(DbSet<T> dbSet)
        {
            _dbSet = dbSet;
        }

        public IQueryable<T> Query => _dbSet;

        public async Task<T> GetByName(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");
            var model = await Query.FirstOrDefaultAsync(x => x.Name.ToLower() == name.ToLower());
            return model;
        }

        public async Task<T> GetById(int id)
        {
            return await Query.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}

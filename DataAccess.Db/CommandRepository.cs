using Cashed.DataAccess.Contract;
using Cashed.DataAccess.Model.Basic;
using System.Data.Entity;

namespace Cashed.DataAccess.Db
{
    public class CommandRepository<T> : ICommandRepository<T> where T : class, IHasId
    {
        private DbSet<T> _dbSet;

        public CommandRepository(DbSet<T> dbSet)
        {
            _dbSet = dbSet;
        }

        public void Create(T model)
        {
            _dbSet.Add(model);
        }

        public void Delete(T model)
        {
            _dbSet.Remove(model);
        }

        public void Update(T model)
        {
            _dbSet.Attach(model);
        }
    }
}

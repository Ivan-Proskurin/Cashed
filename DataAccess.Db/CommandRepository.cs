using Cashed.DataAccess.Contract;
using System.Data.Entity;

namespace Cashed.DataAccess.Db
{
    public class CommandRepository<T> : ICommandRepository<T> where T : class
    {
        private DbSet<T> _dbSet;

        public CommandRepository(DbSet<T> dbSet)
        {
            _dbSet = dbSet;
        }

        public void Create(T model)
        {
            throw new System.NotImplementedException();
        }

        public void Delete(T model)
        {
            throw new System.NotImplementedException();
        }

        public void Update(T model)
        {
            throw new System.NotImplementedException();
        }
    }
}

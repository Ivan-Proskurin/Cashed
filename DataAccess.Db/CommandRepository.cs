using Cashed.DataAccess.Contract;
using System.Data.Entity;
using Cashed.DataAccess.Contract.Base;

namespace Cashed.DataAccess.Db
{
    public class CommandRepository<T> : ICommandRepository<T> where T : class, IHasId
    {
        private readonly CashedDbContext _context;
        private readonly DbSet<T> _dbSet;

        public CommandRepository(CashedDbContext context, DbSet<T> dbSet)
        {
            _context = context;
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
            _context.Entry(model).State = EntityState.Modified;
        }
    }
}

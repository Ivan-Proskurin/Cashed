using Cashed.DataAccess.Contract;
using System;
using System.Data.Entity;
using System.Threading.Tasks;
using Cashed.DataAccess.Contract.Base;

namespace Cashed.DataAccess.Db
{
    public class NamedModelQueryRepository<T> : QueryRepository<T>, INamedModelQueryRepository<T> 
        where T : class, IHasId, IHasName
    {
        public NamedModelQueryRepository(DbSet<T> dbSet) : base(dbSet)
        {
        }

        public async Task<T> GetByName(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));
            var model = await Query.FirstOrDefaultAsync(x => x.Name.ToLower() == name.ToLower());
            return model;
        }
    }
}

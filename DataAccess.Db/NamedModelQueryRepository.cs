using Cashed.DataAccess.Contract;
using Cashed.DataAccess.Model.Basic;
using System;
using System.Data.Entity;
using System.Threading.Tasks;

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
                throw new ArgumentNullException("name");
            var model = await Query.FirstOrDefaultAsync(x => x.Name.ToLower() == name.ToLower());
            return model;
        }
    }
}

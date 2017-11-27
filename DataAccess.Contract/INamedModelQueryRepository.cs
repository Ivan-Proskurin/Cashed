using System.Threading.Tasks;
using Cashed.DataAccess.Contract.Base;

namespace Cashed.DataAccess.Contract
{
    public interface INamedModelQueryRepository<T> : IQueryRepository<T> where T : class, IHasId, IHasName
    {
        Task<T> GetByName(string name);
    }
}

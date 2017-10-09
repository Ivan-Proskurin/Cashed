using Cashed.DataAccess.Model.Basic;
using System.Threading.Tasks;

namespace Cashed.DataAccess.Contract
{
    public interface INamedModelQueryRepository<T> : IQueryRepository<T> where T : class, IHasId, IHasName
    {
        Task<T> GetByName(string name);
    }
}

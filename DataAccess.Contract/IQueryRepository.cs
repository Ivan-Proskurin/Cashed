using System.Linq;
using System.Threading.Tasks;

namespace Cashed.DataAccess.Contract
{
    public interface IQueryRepository<T> where T : class
    {
        IQueryable<T> Query { get; }
        Task<T> GetByName(string name);
    }
}

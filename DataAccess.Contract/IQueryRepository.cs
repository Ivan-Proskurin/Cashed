using System.Linq;

namespace Cashed.DataAccess.Contract
{
    public interface IQueryRepository<T> where T : class
    {
        IQueryable<T> Query { get; }
    }
}

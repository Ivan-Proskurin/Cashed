using Cashed.DataAccess.Model.Basic;
using System.Threading.Tasks;

namespace Cashed.DataAccess.Contract
{
    public interface IUnitOfWork
    {
        IQueryRepository<T> GetQueryRepository<T>() where T : class, IHasName;
        ICommandRepository<T> GetCommandRepository<T>() where T : class, IHasName;
        Task Commit();
    }
}

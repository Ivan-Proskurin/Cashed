using System;
using System.Threading.Tasks;
using Cashed.DataAccess.Contract.Base;

namespace Cashed.DataAccess.Contract
{
    public interface IUnitOfWork : IDisposable
    {
        IQueryRepository<T> GetQueryRepository<T>() where T : class, IHasId;
        INamedModelQueryRepository<T> GetNamedModelQueryRepository<T>() where T : class, IHasId, IHasName;
        ICommandRepository<T> GetCommandRepository<T>() where T : class, IHasId;
        Task Commit();
    }
}

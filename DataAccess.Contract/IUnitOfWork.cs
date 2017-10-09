using Cashed.DataAccess.Model.Basic;
using System.Threading.Tasks;

namespace Cashed.DataAccess.Contract
{
    public interface IUnitOfWork
    {
        IQueryRepository<T> GetQueryRepository<T>() where T : class, IHasId;
        INamedModelQueryRepository<T> GetNamedModelQueryRepository<T>() where T : class, IHasId, IHasName;
        ICommandRepository<T> GetCommandRepository<T>() where T : class, IHasId;
        void UpdateModel<TModel>(TModel model) where TModel : class, IHasId;
        Task Commit();
    }
}

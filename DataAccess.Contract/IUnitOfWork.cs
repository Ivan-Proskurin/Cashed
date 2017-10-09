using Cashed.DataAccess.Model.Basic;
using System.Threading.Tasks;

namespace Cashed.DataAccess.Contract
{
    public interface IUnitOfWork
    {
        IQueryRepository<T> GetQueryRepository<T>() where T : class, IHasName, IHasId;
        ICommandRepository<T> GetCommandRepository<T>() where T : class, IHasName, IHasId;
        void UpdateModel<TModel>(TModel model) where TModel : class, IHasName, IHasId;
        Task Commit();
    }
}

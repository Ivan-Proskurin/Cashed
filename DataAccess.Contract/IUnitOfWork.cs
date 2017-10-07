namespace Cashed.DataAccess.Contract
{
    public interface IUnitOfWork
    {
        IQueryRepository<T> GetQueryRepository<T>() where T : class;
        ICommandRepository<T> GetCommandRepository<T>() where T : class;
        void Commit();
    }
}

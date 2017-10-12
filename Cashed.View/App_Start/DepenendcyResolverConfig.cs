using Cashed.DataAccess.Contract;
using Cashed.DataAccess.Db;
using Logic.Cashed.Contract;
using Logic.Cashed.Logic;
using Ninject;

namespace Cashed.View.App_Start
{
    public class DepenendcyResolverConfig
    {
        private readonly IKernel _kernel;

        public DepenendcyResolverConfig(IKernel kernel)
        {
            _kernel = kernel;
        }

        public void Resolve()
        {
            _kernel.Bind<IUnitOfWork>().To<CashedDatabaseUnitOfWork>();
            _kernel.Bind<ICategoriesQueries>().To<CategoriesQueries>();
            _kernel.Bind<ICategoriesCommands>().To<CategoriesCommands>();
            _kernel.Bind<IExpensesBillQueries>().To<ExpensesBillQueries>();
            _kernel.Bind<IProductQueries>().To<ProductQueries>();
        }
    }
}
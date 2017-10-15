using Cashed.DataAccess.Contract;
using Cashed.DataAccess.Db;
using Logic.Cashed.Contract;
using Logic.Cashed.Logic;
using Ninject;
using Ninject.Web.Common;

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
            _kernel.Bind<IUnitOfWork>().To<CashedDatabaseUnitOfWork>().InRequestScope();
            _kernel.Bind<ICategoriesQueries>().To<CategoriesQueries>();
            _kernel.Bind<ICategoriesCommands>().To<CategoriesCommands>();
            _kernel.Bind<IExpensesBillQueries>().To<ExpensesBillQueries>();
            _kernel.Bind<IProductQueries>().To<ProductQueries>();
            _kernel.Bind<IExpensesBillCommands>().To<ExpensesBillCommands>();
            _kernel.Bind<IProductCommands>().To<ProductCommands>();
            _kernel.Bind<IUserSettings>().To<UserSettings>();
        }
    }
}
using System.Threading.Tasks;
using Cashed.Logic.Contract.Base;
using Cashed.Logic.Contract.Models;

namespace Cashed.Logic.Contract
{
    public interface IExpensesBillCommands : ICommonModelCommands<ExpenseBillModel>
    {
        Task Create(ExpenseBillModel model);
    }
}

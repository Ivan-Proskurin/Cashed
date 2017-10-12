using Logic.Cashed.Contract.Models;
using System.Threading.Tasks;

namespace Logic.Cashed.Contract
{
    public interface IExpensesBillCommands : IGenericModelCommands<ExpenseBillModel>
    {
        Task Create(ExpenseBillModel model);
    }
}

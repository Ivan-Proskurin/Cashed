using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Logic.Cashed.Contract.Models;

namespace Logic.Cashed.Contract
{
    public interface IExpensesBillQueries : ICommonModelQueries<ExpenseBillModel>
    {
        Task<List<ExpenseBillModel>> GetFiltered(DateTime dateFrom, DateTime dateTo);
        TotalsInfoModel GetTotals(List<ExpenseBillModel> bills);
    }
}

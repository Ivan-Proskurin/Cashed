using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cashed.Logic.Contract.Base;
using Cashed.Logic.Contract.Models;

namespace Cashed.Logic.Contract
{
    public interface IExpensesBillQueries : ICommonModelQueries<ExpenseBillModel>
    {
        Task<ExpenseBillsList> GetFiltered(DateTime dateFrom, DateTime dateTo, PaginationArgs args);
        TotalsInfoModel GetTotals(List<ExpenseBillModel> bills);
    }
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Logic.Cashed.Contract.Models;

namespace Logic.Cashed.Contract
{
    public interface IIncomeTypeQueries : ICommonModelQueries<IncomeTypeModel>
    {
        Task<List<IncomeTypeModel>> GetFiltered(DateTime dateFrom, DateTime dateTo);
    }
}
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cashed.Logic.Contract.Base;
using Cashed.Logic.Contract.Models;

namespace Cashed.Logic.Contract
{
    public interface IIncomeTypeQueries : ICommonModelQueries<IncomeTypeModel>
    {
        Task<List<IncomeTypeModel>> GetFiltered(DateTime dateFrom, DateTime dateTo);
    }
}
using System.Collections.Generic;
using Cashed.Logic.Contract.Models;

namespace Cashed.View.Models
{
    public class IncomeTypesListViewModel
    {
        public List<IncomeTypeModel> List { get; set; }
        public IncomesListFilter Filter { get; set; }
        public TotalsInfoModel Total { get; set; }
    }
}
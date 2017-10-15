using System.Collections.Generic;
using Logic.Cashed.Contract.Models;

namespace Cashed.View.Models
{
    public class IncomeTypesListViewModel
    {
        public List<IncomeTypeModel> List { get; set; }
        public IncomesListFilter Filter { get; set; }
        public TotalsInfoModel Total { get; set; }
    }
}
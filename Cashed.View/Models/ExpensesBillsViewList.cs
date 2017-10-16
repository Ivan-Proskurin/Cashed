using System.Collections.Generic;
using Logic.Cashed.Contract.Models;

namespace Cashed.View.Models
{
    public class ExpensesBillsViewList
    {
        public ExpensesListFilterViewModel Filter { get; set; }
        public List<ExpensesBillListViewModel> Bills { get; set; }
        public ExpensesTotalsViewModel Totals { get; set; }
        public PaginationInfo Pagination { get; set; }
    }
}
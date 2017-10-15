using System.Collections.Generic;

namespace Cashed.View.Models
{
    public class ExpensesBillsViewList
    {
        public ExpensesListFilter Filter { get; set; }
        public List<ExpensesBillListViewModel> Bills { get; set; }
        public ExpensesTotalsViewModel Totals { get; set; }
    }
}
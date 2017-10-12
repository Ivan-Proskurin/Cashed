using System.Collections.Generic;

namespace Cashed.View.Models
{
    public class ExpenseBillViewModel
    {
        public List<ExpenseItemViewModel> Subtotals { get; set; }
        public string DateTime { get; set; }
        public string Subtotal { get; set; }
    }
}
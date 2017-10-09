using Cashed.DataAccess.Model.Basic;
using System;
using System.Collections.Generic;

namespace Cashed.DataAccess.Model.Expenses
{
    public class ExpenseBill : IHasId
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public virtual ICollection<ExpenseItem> Items { get; set; }
        public decimal SumPrice { get; set; }
    }
}

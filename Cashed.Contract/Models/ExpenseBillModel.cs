using System;
using System.Collections.Generic;
using System.Linq;

namespace Logic.Cashed.Contract.Models
{
    public class ExpenseBillModel
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public string Category { get; set; }
        public decimal Cost { get; set; }
        public List<ExpenseItemModel> Items { get; set; }

        public void AddItem(ExpenseItemModel item)
        {
            if (Items == null) Items = new List<ExpenseItemModel>();
            Items.Add(item);
            DateTime = Items.Min(x => x.DateTime);
            Cost = Items.Sum(x => x.Cost);
        }
    }
}

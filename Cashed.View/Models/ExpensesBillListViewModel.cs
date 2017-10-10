using System;

namespace Cashed.View.Models
{
    public class ExpensesBillListViewModel
    {
        public int Id { get; set; }
        public string DateTime { get; set; }
        public string Category { get; set; }
        public decimal Cost { get; set; }
    }
}
using System;

namespace Cashed.View.Models
{
    public class ExpensesListFilter
    {
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }

        public ExpensesListFilter()
        {
            
        }

        public ExpensesListFilter(DateTime dateFrom, DateTime dateTo)
        {
            DateFrom = dateFrom;
            DateTo = dateTo;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Cashed.Contract.Models
{
    public class ExpensesBillModel
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public string Category { get; set; }
        public decimal Cost { get; set; }
    }
}

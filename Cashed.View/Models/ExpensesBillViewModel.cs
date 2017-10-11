using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Cashed.View.Models
{
    public class ExpensesBillViewModel
    {
        [Required]
        public string DateTime { get; set; }

        [Required]
        public string Category { get; set; }

        [Required]
        public string Product { get; set; }

        [Required]
        public string Price { get; set; }

        public string Quantity { get; set; }

        public string Comment { get; set; }

        public List<string> AvailCategories { get; set; }
    }
}
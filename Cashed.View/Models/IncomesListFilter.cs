using System.ComponentModel.DataAnnotations;

namespace Cashed.View.Models
{
    public class IncomesListFilter
    {
        [Required]
        public string DateFrom { get; set; }
        [Required]
        public string DateTo { get; set; }
    }
}
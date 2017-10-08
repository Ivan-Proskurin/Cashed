using System.ComponentModel.DataAnnotations;

namespace Cashed.View.Models
{
    public class AddCategoryViewModel
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
    }
}
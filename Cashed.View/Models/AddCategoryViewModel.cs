using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Cashed.View.Models
{
    public class AddCategoryViewModel
    {
        [Required]
        [StringLength(50)]
        [DisplayName("Имя категории")]
        public string Name { get; set; }
    }
}
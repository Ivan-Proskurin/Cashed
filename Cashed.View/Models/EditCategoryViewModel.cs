using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Cashed.View.Models
{
    public class EditCategoryViewModel
    {
        [DisplayName("Номер")]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [DisplayName("Название категории")]
        public string Name { get; set; }
    }
}
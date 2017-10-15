using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Cashed.View.Models
{
    public class EditIncomeTypeViewModel
    {
        [DisplayName("Код")]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [DisplayName("Название статьи")]
        public string Name { get; set; }
    }
}
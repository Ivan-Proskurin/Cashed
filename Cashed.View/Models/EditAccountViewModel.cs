using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Cashed.View.Models
{
    public class EditAccountViewModel
    {
        [DisplayName("Код")]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [DisplayName("Название счета")]
        public string Name { get; set; }

        [Required]
        [DisplayName("Баланс")]
        public string Balance { get; set; }
    }
}
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Logic.Cashed.Contract.Models;

namespace Cashed.View.Models
{
    public class CategoryListViewModel
    {
        public CategoryList List { get; set; }
        [Required]
        [StringLength(50)]
        [DisplayName("Название категории")]
        public string Category { get; set; }
    }
}
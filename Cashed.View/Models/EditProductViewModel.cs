using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Cashed.Logic.Contract.Models;

namespace Cashed.View.Models
{
    public class EditProductViewModel
    {
        [DisplayName("Код")]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [DisplayName("Название")]
        public string Name { get; set; }

        public int CategoryId { get; set; }

        [Required]
        [DisplayName("Категория")]
        public string CategoryName { get; set; }

        public List<CategoryModel> Categories { get; set; }
    }
}
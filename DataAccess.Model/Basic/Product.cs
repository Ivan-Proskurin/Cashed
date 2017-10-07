using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cashed.DataAccess.Model
{
    public class Product
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }
    }
}

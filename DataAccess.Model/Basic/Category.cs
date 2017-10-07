using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Cashed.DataAccess.Model
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}

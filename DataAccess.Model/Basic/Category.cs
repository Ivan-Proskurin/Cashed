using Cashed.DataAccess.Model.Basic;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Cashed.DataAccess.Model
{
    public class Category : IHasName, IHasId
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public bool IsDeleted { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}

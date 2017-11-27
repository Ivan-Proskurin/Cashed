using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Cashed.DataAccess.Contract.Base;

namespace Cashed.DataAccess.Model.Base
{
    public class Product : IHasName, IHasId
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }
        public bool IsDeleted { get; set; }
    }
}

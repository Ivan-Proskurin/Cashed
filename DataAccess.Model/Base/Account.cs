using System.ComponentModel.DataAnnotations;
using Cashed.DataAccess.Contract.Base;

namespace Cashed.DataAccess.Model.Base
{
    public class Account : IHasId, IHasName
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public decimal Balance { get; set; }

        public bool IsDeleted { get; set; }
    }
}
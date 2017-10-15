using System.ComponentModel.DataAnnotations;
using Cashed.DataAccess.Model.Basic;

namespace Cashed.DataAccess.Model.Incomes
{
    public class IncomeType : IHasId, IHasName
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
    }
}

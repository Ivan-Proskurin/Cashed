using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Cashed.Logic.Contract.Models;

namespace Cashed.View.Models
{
    public class AddIncomeViewModel
    {
        [Required]
        [DisplayName("Статья дохода")]
        public string IncomeType { get; set; }
        [Required]
        [DisplayName("Дата/время")]
        public string DateTime { get; set; }
        [Required]
        [DisplayName("Сумма")]
        public string Total { get; set; }

        public List<IncomeTypeModel> IncomeTypes { get; set; }
    }
}
using System;
using System.ComponentModel.DataAnnotations.Schema;
using Cashed.DataAccess.Model.Basic;

namespace Cashed.DataAccess.Model.Incomes
{
    public class IncomeItem : IHasId
    {
        public int Id { get; set; }
        public int IncomeTypeId { get; set; }
        [ForeignKey("IncomeTypeId")]
        public IncomeType IncomeType { get; set; }

        public DateTime DateTime { get; set; }

        public decimal Total { get; set; }
    }
}
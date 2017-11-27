using System.Collections.Generic;

namespace Cashed.Logic.Contract.Models
{
    public class ExpenseBillsList
    {
        public List<ExpenseBillModel> List { get; set; }
        public PaginationInfo Pagination { get; set; }
    }
}

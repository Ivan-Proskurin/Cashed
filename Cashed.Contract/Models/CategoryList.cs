using System.Collections.Generic;

namespace Cashed.Logic.Contract.Models
{
    public class CategoryList
    {
        public List<CategoryModel> List { get; set; }
        public PaginationInfo Pagination { get; set; }
    }
}
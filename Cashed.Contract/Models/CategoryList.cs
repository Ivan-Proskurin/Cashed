using System.Collections.Generic;

namespace Logic.Cashed.Contract.Models
{
    public class CategoryList
    {
        public List<CategoryModel> List { get; set; }
        public PaginationInfo Pagination { get; set; }
    }
}
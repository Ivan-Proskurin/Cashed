using System.Collections.Generic;

namespace Logic.Cashed.Contract.Models
{
    public class ProductList
    {
        public List<ProductModel> Products { get; set; }
        public PaginationInfo Pagination { get; set; }
    }
}
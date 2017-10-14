using System.Collections.Generic;
using Logic.Cashed.Contract.Models;

namespace Cashed.View.Models
{
    public class ProductListViewModel
    {
        public string CategoryName { get; set; }
        public List<CategoryModel> Categories { get; set; }
    }
}
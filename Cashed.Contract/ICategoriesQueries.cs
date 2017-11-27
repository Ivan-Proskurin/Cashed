using System.Collections.Generic;
using System.Threading.Tasks;
using Cashed.Logic.Contract.Base;
using Cashed.Logic.Contract.Models;

namespace Cashed.Logic.Contract
{
    public interface ICategoriesQueries : ICommonModelQueries<CategoryModel>
    {
        Task<List<ProductModel>> GetProductsByCategoryName(string categoryName, bool includeDeleted = false);
        Task<CategoryList> GetList(PaginationArgs args);
    }
}

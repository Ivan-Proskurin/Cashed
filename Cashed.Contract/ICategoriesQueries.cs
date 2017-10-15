using Logic.Cashed.Contract.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Logic.Cashed.Contract
{
    public interface ICategoriesQueries : ICommonModelQueries<CategoryModel>
    {
        Task<List<ProductModel>> GetProductsByCategoryName(string categoryName, bool includeDeleted = false);
        Task<CategoryList> GetList(GetModelListArgs args);
    }
}

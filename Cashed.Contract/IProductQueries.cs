using System.Collections.Generic;
using Logic.Cashed.Contract.Models;
using System.Threading.Tasks;

namespace Logic.Cashed.Contract
{
    public interface IProductQueries : ICommonModelQueries<ProductModel>
    {
        Task<List<ProductModel>> GetCategoryProducts(int categoryId, bool includeDeleted = false);
    }
}

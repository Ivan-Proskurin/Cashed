using System.Collections.Generic;
using System.Threading.Tasks;
using Cashed.Logic.Contract.Base;
using Cashed.Logic.Contract.Models;

namespace Cashed.Logic.Contract
{
    public interface IProductQueries : ICommonModelQueries<ProductModel>
    {
        Task<List<ProductModel>> GetCategoryProducts(int categoryId, bool includeDeleted = false);
    }
}

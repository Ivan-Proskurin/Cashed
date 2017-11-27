using System.Collections.Generic;
using System.Threading.Tasks;
using Cashed.Logic.Contract.Base;
using Cashed.Logic.Contract.Models;

namespace Cashed.Logic.Contract
{
    public interface IProductCommands : ICommonModelCommands<ProductModel>
    {
        Task<ProductModel> AddProductToCategory(int categoryId, string productName);
        Task<List<int>> GroupDeletion(int[] ids, bool onlyMark = true);
    }
}
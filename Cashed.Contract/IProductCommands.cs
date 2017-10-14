using System.Collections.Generic;
using System.Threading.Tasks;
using Logic.Cashed.Contract.Models;

namespace Logic.Cashed.Contract
{
    public interface IProductCommands : IGenericModelCommands<ProductModel>
    {
        Task<ProductModel> AddProductToCategory(int categoryId, string productName);
        Task<List<int>> GroupDeletion(int[] ids);
    }
}
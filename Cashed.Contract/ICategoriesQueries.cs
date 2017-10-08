using Logic.Cashed.Contract.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Logic.Cashed.Contract
{
    public interface ICategoriesQueries
    {
        Task<List<CategoryModel>> GetAllCategories();
    }
}

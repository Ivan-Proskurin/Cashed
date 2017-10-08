using System.Collections.Generic;
using Logic.Cashed.Contract;
using Logic.Cashed.Contract.Models;
using System.Threading.Tasks;
using Cashed.DataAccess.Contract;
using Cashed.DataAccess.Model;
using System.Data.Entity;
using System.Linq;

namespace Logic.Cashed.Logic
{
    public class CategoriesQueries : ICategoriesQueries
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoriesQueries(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<CategoryModel>> GetAllCategories()
        {
            var repo = _unitOfWork.GetQueryRepository<Category>();
            return await repo.Query.Select(x => new CategoryModel
            {
                Id = x.Id,
                Name = x.Name
            }
            ).ToListAsync();
        }
    }
}

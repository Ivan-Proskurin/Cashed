using System.Collections.Generic;
using Logic.Cashed.Contract;
using Logic.Cashed.Contract.Models;
using System.Threading.Tasks;
using Cashed.DataAccess.Contract;
using Cashed.DataAccess.Model;
using System.Data.Entity;
using System.Linq;
using System;

namespace Logic.Cashed.Logic
{
    public class CategoriesQueries : ICategoriesQueries
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoriesQueries(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<CategoryModel>> GetAll()
        {
            var repo = _unitOfWork.GetQueryRepository<Category>();
            return await repo.Query.Select(x => new CategoryModel
            {
                Id = x.Id,
                Name = x.Name
            }
            ).ToListAsync();
        }

        public async Task<CategoryModel> GetById(int id)
        {
            var repo = _unitOfWork.GetQueryRepository<Category>();
            return await repo.Query.Where(x => x.Id == id)
                .Select(x => new CategoryModel
                {
                    Id = x.Id,
                    Name = x.Name
                }
            ).FirstOrDefaultAsync();
        }

        public async Task<CategoryModel> GetByName(string name)
        {
            var category = await _unitOfWork.GetNamedModelQueryRepository<Category>().GetByName(name);
            if (category == null) return null;
            return new CategoryModel
            {
                Id = category.Id,
                Name = category.Name
            };
        }

        public async Task<List<ProductModel>> GetProductsByCategoryName(string categoryName)
        {
            var category = await _unitOfWork.GetNamedModelQueryRepository<Category>().GetByName(categoryName);
            if (category == null)
                throw new ArgumentException($"Нет категории с названием \"{categoryName}\"");

            var productsRepo = _unitOfWork.GetQueryRepository<Product>();
            var products = await productsRepo.Query
                .Where(x => x.CategoryId == category.Id)
                .Select(x => new ProductModel
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .ToListAsync();
            return products;
        }
    }
}

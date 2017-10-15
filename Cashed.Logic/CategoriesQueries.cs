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

        public async Task<List<CategoryModel>> GetAll(bool includeDeleted = false)
        {
            var repo = _unitOfWork.GetQueryRepository<Category>();
            var query = includeDeleted ? repo.Query : repo.Query.Where(x => !x.IsDeleted);
            return await query
                .Select(x => new CategoryModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    ProductCount = x.Products.Count
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
                    Name = x.Name,
                    ProductCount = x.Products.Count
                }
            ).FirstOrDefaultAsync();
        }

        public async Task<CategoryModel> GetByName(string name, bool includeDeleted = false)
        {
            var category = await _unitOfWork.GetNamedModelQueryRepository<Category>().GetByName(name);
            if (category == null || category.IsDeleted && !includeDeleted) return null;
            return new CategoryModel
            {
                Id = category.Id,
                Name = category.Name,
                ProductCount = category.Products.Count
            };
        }

        public async Task<List<ProductModel>> GetProductsByCategoryName(string categoryName, bool includeDeleted = false)
        {
            var category = await _unitOfWork.GetNamedModelQueryRepository<Category>().GetByName(categoryName);
            if (category == null)
                throw new ArgumentException($"Нет категории с названием \"{categoryName}\"");

            var productsRepo = _unitOfWork.GetQueryRepository<Product>();
            var query = includeDeleted ? productsRepo.Query : productsRepo.Query.Where(x => !x.IsDeleted);
            var products = await query
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

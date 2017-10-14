using Logic.Cashed.Contract;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Logic.Cashed.Contract.Models;
using Cashed.DataAccess.Contract;
using Cashed.DataAccess.Model;

namespace Logic.Cashed.Logic
{
    public class ProductQueries : IProductQueries
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductQueries(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<List<ProductModel>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<ProductModel> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ProductModel> GetByName(string name)
        {
            var productRepo = _unitOfWork.GetNamedModelQueryRepository<Product>();
            var product = await productRepo.GetByName(name);
            if (product == null) return null;
            return new ProductModel
            {
                Id = product.Id,
                CategoryId = product.CategoryId,
                Name = product.Name
            };
        }

        public async Task<List<ProductModel>> GetCategoryProducts(int categoryId)
        {
            var productRepo = _unitOfWork.GetQueryRepository<Product>();
            return await productRepo.Query
                .Where(x => x.CategoryId == categoryId)
                .Select(x => new ProductModel
                {
                    Id = x.Id,
                    CategoryId = x.CategoryId,
                    Name = x.Name
                }).ToListAsync();
        }
    }
}

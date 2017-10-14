using Logic.Cashed.Contract;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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

        public Task<List<ProductModel>> GetAll(bool includeDeleted = false)
        {
            throw new NotImplementedException();
        }

        public async Task<ProductModel> GetById(int id)
        {
            var prodRepo = _unitOfWork.GetQueryRepository<Product>();
            var product = await prodRepo.GetById(id);
            if (product == null)
                throw new ArgumentException($"Нет продукта с идентификатором {id}");
            return new ProductModel
            {
                Id = product.Id,
                CategoryId = product.CategoryId,
                Name = product.Name
            };
        }

        public async Task<ProductModel> GetByName(string name, bool includeDeleted = false)
        {
            var productRepo = _unitOfWork.GetNamedModelQueryRepository<Product>();
            var product = await productRepo.GetByName(name);
            if (product == null || product.IsDeleted && !includeDeleted) return null;
            return new ProductModel
            {
                Id = product.Id,
                CategoryId = product.CategoryId,
                Name = product.Name
            };
        }

        public async Task<List<ProductModel>> GetCategoryProducts(int categoryId, bool includeDeleted = false)
        {
            var productRepo = _unitOfWork.GetQueryRepository<Product>();
            var query = includeDeleted ? productRepo.Query : productRepo.Query.Where(x => !x.IsDeleted);
            return await query
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

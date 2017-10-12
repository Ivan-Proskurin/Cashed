using Logic.Cashed.Contract;
using System;
using System.Collections.Generic;
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
            if (product == null)
                throw new ArgumentException($"Нет продукта с названием {name}");
            return new ProductModel
            {
                Id = product.Id,
                Name = product.Name
            };
        }
    }
}

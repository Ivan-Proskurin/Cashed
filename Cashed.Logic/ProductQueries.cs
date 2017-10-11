using Logic.Cashed.Contract;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Logic.Cashed.Contract.Models;

namespace Logic.Cashed.Logic
{
    public class ProductQueries : IProductQueries
    {
        public Task<List<ProductModel>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<ProductModel> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ProductModel> GetByName(string name)
        {
            throw new NotImplementedException();
        }
    }
}

using System.Collections.Generic;
using System.Threading.Tasks;
using Cashed.DataAccess.Contract;
using Cashed.DataAccess.Model;
using Logic.Cashed.Contract;
using Logic.Cashed.Contract.Models;

namespace Logic.Cashed.Logic
{
    public class ProductCommands : IProductCommands
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductCommands(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task Update(ProductModel model)
        {
            throw new System.NotImplementedException();
        }

        public Task Delete(int id, bool onlyMark = true)
        {
            throw new System.NotImplementedException();
        }

        public async Task<ProductModel> AddProductToCategory(int categoryId, string productName)
        {
            var commands = _unitOfWork.GetCommandRepository<Product>();
            var model = new Product
            {
                Id = -1,
                CategoryId = categoryId,
                Name = productName
            };
            commands.Create(model);
            await _unitOfWork.Commit();
            return new ProductModel
            {
                Id = model.Id,
                CategoryId = model.CategoryId,
                Name = model.Name
            };
        }

        public async Task<List<int>> GroupDeletion(int[] ids, bool onlyMark = true)
        {
            var deletedList = new List<int>();
            var queries = _unitOfWork.GetQueryRepository<Product>();
            var commands = _unitOfWork.GetCommandRepository<Product>();
            foreach (var id in ids)
            {
                var model = await queries.GetById(id);
                if (model == null) continue;
                if (onlyMark)
                {
                    model.IsDeleted = true;
                    _unitOfWork.UpdateModel(model);
                }
                else
                {
                    // todo: удалить все операции по продукту
                    commands.Delete(model);
                }
                deletedList.Add(id);
            }
            await _unitOfWork.Commit();
            return deletedList;
        }
    }
}
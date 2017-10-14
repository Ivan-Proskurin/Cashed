using Cashed.DataAccess.Contract;
using Cashed.DataAccess.Model;
using Logic.Cashed.Contract;
using Logic.Cashed.Contract.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Logic.Cashed.Logic
{
    public class CategoriesCommands : ICategoriesCommands
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductCommands _productCommands;
        private readonly IProductQueries _productQueries;

        public CategoriesCommands(IUnitOfWork unitOfWork, IProductCommands productCommands, IProductQueries productQueries)
        {
            _unitOfWork = unitOfWork;
            _productCommands = productCommands;
            _productQueries = productQueries;
        }

        public async Task Delete(int id, bool onlyMark = true)
        {
            var categoriesCommands = _unitOfWork.GetCommandRepository<Category>();
            var categoriesQueries = _unitOfWork.GetQueryRepository<Category>();
            var category = await categoriesQueries.GetById(id);
            if (category == null)
                throw new ArgumentException($"Нет категории с идентификатором {id}");
            var products = await _productQueries.GetCategoryProducts(category.Id, true);
            if (onlyMark)
            {
                await _productCommands.GroupDeletion(products.Select(x => x.Id).ToArray());
                category.IsDeleted = true;
                _unitOfWork.UpdateModel(category);
            }
            else
            {
                await _productCommands.GroupDeletion(products.Select(x => x.Id).ToArray(), false);
                categoriesCommands.Delete(category);
            }
            await _unitOfWork.Commit();
        }

        public async Task Update(CategoryModel model)
        {
            var categoryRespoitory = _unitOfWork.GetCommandRepository<Category>();
            if (model.Id > 0)
            {
                _unitOfWork.UpdateModel(new Category
                {
                    Id = model.Id,
                    Name = model.Name
                });
            }
            else
            {
                var categoriesQueries = _unitOfWork.GetNamedModelQueryRepository<Category>();
                if (await categoriesQueries.GetByName(model.Name) != null)
                    throw new ArgumentException("Категория с таким именем уже существует");
                categoryRespoitory.Create(
                    new Category
                    {
                        Id = -1,
                        Name = model.Name
                    });
            }
            await _unitOfWork.Commit();
        }
    }
}

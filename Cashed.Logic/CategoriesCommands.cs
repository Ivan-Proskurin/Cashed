using Cashed.DataAccess.Contract;
using Cashed.DataAccess.Model;
using Logic.Cashed.Contract;
using Logic.Cashed.Contract.Models;
using System;
using System.Threading.Tasks;

namespace Logic.Cashed.Logic
{
    public class CategoriesCommands : ICategoriesCommands
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoriesCommands(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void Delete(int id)
        {
            throw new System.NotImplementedException();
        }

        public async Task Update(CategoryModel model)
        {
            var categoriesQueries = _unitOfWork.GetQueryRepository<Category>();
            if (await categoriesQueries.GetByName(model.Name) != null)
                throw new ArgumentException("Категория с таким именем уже существует");
            _unitOfWork.GetCommandRepository<Category>().Create(
                new Category
                {
                    Id = -1,
                    Name = model.Name
                });
            await _unitOfWork.Commit();
        }
    }
}

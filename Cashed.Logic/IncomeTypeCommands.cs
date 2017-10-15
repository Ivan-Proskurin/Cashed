using System.Threading.Tasks;
using Cashed.DataAccess.Contract;
using Cashed.DataAccess.Model.Incomes;
using Logic.Cashed.Contract;
using Logic.Cashed.Contract.Models;

namespace Logic.Cashed.Logic
{
    public class IncomeTypeCommands : IIncomeTypeCommands
    {
        private readonly IUnitOfWork _unitOfWork;

        public IncomeTypeCommands(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IncomeTypeModel> Update(IncomeTypeModel model)
        {
            var type = new IncomeType
            {
                Id = model.Id,
                Name = model.Name
            };
            if (type.Id > 0)
            {
                _unitOfWork.UpdateModel(type);
            }
            else
            {
                var typeRepo = _unitOfWork.GetCommandRepository<IncomeType>();
                typeRepo.Create(type);
            }
            await _unitOfWork.Commit();
            model.Id = type.Id;
            return model;
        }

        public Task Delete(int id, bool onlyMark = true)
        {
            throw new System.NotImplementedException();
        }
    }
}
using System.Threading.Tasks;
using Cashed.DataAccess.Contract;
using Cashed.DataAccess.Model.Incomes;
using Cashed.Logic.Contract;
using Cashed.Logic.Contract.Models;

namespace Cashed.Logic
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
            var typeRepo = _unitOfWork.GetCommandRepository<IncomeType>();
            var type = new IncomeType
            {
                Id = model.Id,
                Name = model.Name
            };
            if (type.Id > 0)
            {
                typeRepo.Update(type);
            }
            else
            {
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
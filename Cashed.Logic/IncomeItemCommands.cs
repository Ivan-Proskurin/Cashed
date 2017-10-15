using System.Threading.Tasks;
using Cashed.DataAccess.Contract;
using Cashed.DataAccess.Model.Incomes;
using Logic.Cashed.Contract;
using Logic.Cashed.Contract.Models;

namespace Logic.Cashed.Logic
{
    public class IncomeItemCommands : IIncomeItemCommands
    {
        private readonly IUnitOfWork _unitOfWork;

        public IncomeItemCommands(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task Delete(int id, bool onlyMark = true)
        {
            throw new System.NotImplementedException();
        }

        public async Task<IncomeItemModel> Update(IncomeItemModel model)
        {
            var item = new IncomeItem
            {
                Id = model.Id,
                DateTime = model.DateTime,
                IncomeTypeId = model.IncomeTypeId,
                Total = model.Total
            };
            if (item.Id > 0)
            {
                _unitOfWork.UpdateModel(item);
            }
            else
            {
                var itemRepo = _unitOfWork.GetCommandRepository<IncomeItem>();
                itemRepo.Create(item);
            }
            await _unitOfWork.Commit();
            model.Id = item.Id;
            return model;
        }
    }
}
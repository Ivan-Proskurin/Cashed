using Logic.Cashed.Contract;
using System;
using System.Threading.Tasks;
using Logic.Cashed.Contract.Models;
using Cashed.DataAccess.Contract;
using Cashed.DataAccess.Model.Expenses;

namespace Logic.Cashed.Logic
{
    public class ExpensesBillCommands : IExpensesBillCommands
    {
        private readonly IUnitOfWork _unitOfWork;

        public ExpensesBillCommands(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task Delete(int id, bool onlyMark = true)
        {
            throw new NotImplementedException();
        }

        public async Task<ExpenseBillModel> Update(ExpenseBillModel model)
        {
            var itemsRepo = _unitOfWork.GetCommandRepository<ExpenseItem>();
            foreach (var item in model.Items)
            {
                if (item.Id > 0) continue;
                itemsRepo.Create(new ExpenseItem
                {
                    Id = -1,
                    BillId = model.Id,
                    CategoryId = item.CategoryId,
                    ProductId = item.ProductId,
                    DateTime = item.DateTime,
                    Price = item.Cost,
                    Quantity = item.Quantity,
                    Comment = item.Comment
                });
            }

            await _unitOfWork.Commit();

            return model;
        }

        public async Task Create(ExpenseBillModel model)
        {
            var billRepo = _unitOfWork.GetCommandRepository<ExpenseBill>();

            var bill = new ExpenseBill
            {
                Id = -1,
                DateTime = model.DateTime,
                SumPrice = model.Cost
            };

            billRepo.Create(bill);

            var itemsRepo = _unitOfWork.GetCommandRepository<ExpenseItem>();

            foreach (var item in model.Items)
            {
                itemsRepo.Create(new ExpenseItem
                {
                    Id = -1,
                    Bill = bill,
                    CategoryId = item.CategoryId,
                    ProductId = item.ProductId,
                    DateTime = item.DateTime,
                    Price = item.Cost,
                    Quantity = item.Quantity,
                    Comment = item.Comment
                });
            }

            await _unitOfWork.Commit();
        }
    }
}

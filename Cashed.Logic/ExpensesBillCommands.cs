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
                if (item.Id > 0 && !item.IsModified && !item.IsDeleted) continue;
                if (item.Id < 0 || item.IsModified)
                {
                    var itemModel = new ExpenseItem
                    {
                        Id = item.Id,
                        BillId = model.Id,
                        CategoryId = item.CategoryId,
                        ProductId = item.ProductId,
                        DateTime = item.DateTime,
                        Price = item.Cost,
                        Quantity = item.Quantity,
                        Comment = item.Comment
                    };
                    if (item.Id < 0)
                        itemsRepo.Create(itemModel);
                    else
                        _unitOfWork.UpdateModel(itemModel);
                }
                else if (item.IsDeleted)
                {
                    var itemModel = await _unitOfWork.GetQueryRepository<ExpenseItem>().GetById(item.Id);
                    if (itemModel == null) continue;
                    itemsRepo.Delete(itemModel);
                }
            }

            _unitOfWork.UpdateModel(new ExpenseBill
            {
                Id = model.Id,
                DateTime = model.DateTime,
                SumPrice = model.Cost
            });

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

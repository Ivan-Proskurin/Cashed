using System.Collections.Generic;
using System.Threading.Tasks;
using Logic.Cashed.Contract;
using Logic.Cashed.Contract.Models;
using Cashed.DataAccess.Contract;
using Cashed.DataAccess.Model.Expenses;
using System.Linq;
using System.Data.Entity;
using Cashed.DataAccess.Model;
using System;

namespace Logic.Cashed.Logic
{
    public class ExpensesBillQueries : IExpensesBillQueries
    {
        private readonly IUnitOfWork _unitOfWork;

        public ExpensesBillQueries(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<List<ExpenseBillModel>> GetAll(bool includeDeleted = false)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ExpenseBillModel>> GetFiltered(DateTime dateFrom, DateTime dateTo)
        {
            // отбираем нужные счета
            var expenseRepo = _unitOfWork.GetQueryRepository<ExpenseBill>();
            var bills = await expenseRepo.Query
                .Where(x => x.DateTime >= dateFrom && x.DateTime < dateTo)
                .Select(x => new ExpenseBillModel
                {
                    Id = x.Id,
                    Cost = x.SumPrice,
                    DateTime = x.DateTime
                })
                .ToListAsync();
            var billIds = bills.Select(x => x.Id).ToList();

            // распределяем счет на самые увестистые категории расходов и группируем по ним
            var itemsRepo = _unitOfWork.GetQueryRepository<ExpenseItem>();

            var groupByCategoryQuery =
                from itm in itemsRepo.Query
                where billIds.Contains(itm.BillId)
                group itm.Price by new { itm.BillId, itm.CategoryId }
                into itmGroup
                select new
                {
                    BillId = itmGroup.Key.BillId,
                    CategoryId = itmGroup.Key.CategoryId,
                    Total = itmGroup.Sum()
                };
            var groupByCategory = await groupByCategoryQuery
                .OrderByDescending(x => x.Total).ToListAsync();

            var categoriesRepo = _unitOfWork.GetQueryRepository<Category>();
            var models = new List<ExpenseBillModel>();
            foreach (var billCategory in groupByCategory)
            {
                var category = await categoriesRepo.GetById(billCategory.CategoryId);

                var bill = new ExpenseBillModel()
                {
                    Id = billCategory.BillId,
                    DateTime = bills.First(x => x.Id == billCategory.BillId).DateTime,
                    Cost = billCategory.Total,
                    Category = category.Name
                };
                models.Add(bill);
            }

            return models.OrderByDescending(x => x.DateTime).ThenByDescending(x => x.Cost).ToList();
        }

        public async Task<ExpenseBillModel> GetById(int id)
        {
            var repo = _unitOfWork.GetQueryRepository<ExpenseBill>();
            var bill = await repo.GetById(id);
            var model = new ExpenseBillModel
            {
                Id = bill.Id,
                DateTime = bill.DateTime,
                Cost = bill.SumPrice,
                Items = bill.Items.Select(x => new ExpenseItemModel
                {
                    Id = x.Id,
                    CategoryId = x.CategoryId,
                    DateTime = x.DateTime,
                    Comment = x.Comment,
                    Cost = x.Price,
                    ProductId = x.ProductId,
                    Quantity = x.Quantity
                }).ToList()
            };
            await FullfillItemsFields(model);
            return model;
        }

        private async Task FullfillItemsFields(ExpenseBillModel bill)
        {
            var catRepo = _unitOfWork.GetQueryRepository<Category>();
            var prodRepo = _unitOfWork.GetQueryRepository<Product>();
            foreach (var item in bill.Items)
            {
                var product = await prodRepo.GetById(item.ProductId);
                item.Product = product.Name;
                var category = await catRepo.GetById(item.CategoryId);
                item.Category = category.Name;
            }
        }

        public Task<ExpenseBillModel> GetByName(string name, bool includeDeleted = false)
        {
            throw new System.NotImplementedException();
        }

        public TotalsInfoModel GetTotals(List<ExpenseBillModel> bills)
        {
            if (bills.Count > 0)
            {
                var orderedBills = bills.OrderBy(x => x.DateTime).ToList();
                var totals = new TotalsInfoModel
                {
                    Caption = $"Итог за {orderedBills.FirstOrDefault()?.DateTime: yyyy.MM.dd} - {orderedBills.LastOrDefault()?.DateTime: yyyy.MM.dd}: ",
                    Total = bills.Sum(x => x.Cost)
                };
                return totals;
            }
            return new TotalsInfoModel
            {
                Caption = "Итог:",
                Total = 0m
            };
        }
    }
}

using System.Collections.Generic;
using System.Threading.Tasks;
using Logic.Cashed.Contract;
using Logic.Cashed.Contract.Models;
using Cashed.DataAccess.Contract;
using Cashed.DataAccess.Model.Expenses;
using System.Linq;
using System.Data.Entity;
using Cashed.DataAccess.Model;

namespace Logic.Cashed.Logic
{
    public class ExpensesBillQueries : IExpensesBillQueries
    {
        private readonly IUnitOfWork _unitOfWork;

        public ExpensesBillQueries(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<ExpensesBillModel>> GetAll()
        {
            // отбираем нужные счета
            var expenseRepo = _unitOfWork.GetQueryRepository<ExpenseBill>();
            var bills = await expenseRepo.Query
                .Select(x => new ExpensesBillModel
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
            var models = new List<ExpensesBillModel>();
            foreach (var billCategory in groupByCategory)
            {
                var category = await categoriesRepo.GetById(billCategory.CategoryId);

                var bill = new ExpensesBillModel()
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

        public Task<ExpensesBillModel> GetById(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<ExpensesBillModel> GetByName(string name)
        {
            throw new System.NotImplementedException();
        }
    }
}

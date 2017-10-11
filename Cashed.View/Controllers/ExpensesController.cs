using Cashed.View.Models;
using Logic.Cashed.Contract;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Cashed.View.Controllers
{
    public class ExpensesController : Controller
    {
        private readonly IExpensesBillQueries _expensesBillQueries;
        private readonly ICategoriesQueries _categoriesQueries;

        public ExpensesController(IExpensesBillQueries expensesBillQueries,
            ICategoriesQueries categoriesQueries)
        {
            _expensesBillQueries = expensesBillQueries;
            _categoriesQueries = categoriesQueries;
        }

        // GET: Expenses
        public async Task<ActionResult> Index()
        {
            var models = await _expensesBillQueries.GetAll();
            return View(new ExpensesBillsViewList()
            {
                Bills = models.Select(x => new ExpensesBillListViewModel
                {
                    Id = x.Id,
                    DateTime = x.DateTime.ToString("dd.MM.yyyy HH:mm"),
                    Category = x.Category,
                    Cost = x.Cost
                }).ToList()
            });
        }

        public async Task<ActionResult> Add()
        {
            var model = new ExpensesBillViewModel
            {
                DateTime = DateTime.Now.ToString("yyyy.MM.dd HH:mm")
            };
            var categories = await _categoriesQueries.GetAll();
            model.AvailCategories = categories.Select(x => x.Name).OrderBy(x => x).ToList();
            return View(model);
        }

        public async Task<JsonResult> GetCategoryProducts(string category)
        {
            var products = await _categoriesQueries.GetProductsByCategoryName(category);
            return Json(products.Select(x => x.Name).OrderBy(x => x).ToList(), JsonRequestBehavior.AllowGet);
        }
    }
}
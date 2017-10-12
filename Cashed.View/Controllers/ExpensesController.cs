using Cashed.View.Models;
using Logic.Cashed.Contract;
using Logic.Cashed.Contract.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Cashed.View.Controllers
{
    public class ExpensesController : Controller
    {
        private static readonly string CurrentBillSessionName = "CurrentBill";

        private readonly IExpensesBillQueries _expensesBillQueries;
        private readonly ICategoriesQueries _categoriesQueries;
        private readonly IProductQueries _productQueries;
        private readonly IExpensesBillCommands _expensesBillCommands;

        public ExpensesController(IExpensesBillQueries expensesBillQueries,
            ICategoriesQueries categoriesQueries, IProductQueries productQueries,
            IExpensesBillCommands expensesBillCommands)
        {
            _expensesBillQueries = expensesBillQueries;
            _categoriesQueries = categoriesQueries;
            _productQueries = productQueries;
            _expensesBillCommands = expensesBillCommands;
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

        public async Task<ActionResult> Add(string datetime = null, string category = null, bool noItems = false)
        {
            return View(await CreateExpenseItemViewModel(datetime, category, noItems));
        }

        private async Task<ExpenseItemViewModel> CreateExpenseItemViewModel(string datetime, string category, bool noItems)
        {
            var model = new ExpenseItemViewModel
            {
                DateTime = datetime ?? DateTime.Now.ToString("yyyy.MM.dd HH:00"),
                Category = category,
                NoItems = noItems
            };
            model.AvailCategories = await GetAllCategoriesNames();
            return model;
        }

        private async Task<List<string>> GetAllCategoriesNames()
        {
            var categories = await _categoriesQueries.GetAll();
            return categories.Select(x => x.Name).OrderBy(x => x).ToList();
        }

        [HttpPost]
        public async Task<ActionResult> Add(ExpenseItemViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var expenseItem = await ExpenseItemViewModelToModel(model);
                    var bill = GetCurrentBill();
                    bill.AddItem(expenseItem);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    model.AvailCategories = await GetAllCategoriesNames();
                    return View(model);
                }
                return RedirectToAction("Add", new { datetime = model.DateTime, category = model.Category });
            }
            else
            {
                model.AvailCategories = await GetAllCategoriesNames();
                return View(model);
            }
        }

        private async Task<ExpenseItemModel> ExpenseItemViewModelToModel(ExpenseItemViewModel viewModel)
        {
            var category = await _categoriesQueries.GetByName(viewModel.Category);
            var product = await _productQueries.GetByName(viewModel.Product);
            var model = new ExpenseItemModel
            {
                Id = -1,
                DateTime = DateTime.ParseExact(viewModel.DateTime, "yyyy.MM.dd HH:mm", CultureInfo.InvariantCulture),
                CategoryId = category.Id,
                Category = category.Name,
                ProductId = product.Id,
                Product = product.Name,
                Cost = decimal.Parse(viewModel.Price.Replace(',', '.'), CultureInfo.InvariantCulture),
                Quantity = string.IsNullOrWhiteSpace(viewModel.Quantity) ?
                    1 :
                    decimal.Parse(viewModel.Quantity.Replace(',', '.'), CultureInfo.InvariantCulture),
                Comment = viewModel.Comment
            };

            return model;
        }

        public async Task<JsonResult> GetCategoryProducts(string category)
        {
            var products = await _categoriesQueries.GetProductsByCategoryName(category);
            return Json(products.Select(x => x.Name).OrderBy(x => x).ToList(), JsonRequestBehavior.AllowGet);
        }

        private ExpenseBillModel GetCurrentBill()
        {
            var bill = Session[CurrentBillSessionName] as ExpenseBillModel;
            if (bill == null)
            {
                bill = new ExpenseBillModel
                {
                    Id = -1,
                    Items = new List<ExpenseItemModel>()
                };
                Session[CurrentBillSessionName] = bill;
            }
            return bill;
        }

        public ActionResult GetSubtotal()
        {
            var culture = new CultureInfo("ru-ru");
            var bill = GetCurrentBill();
            var subtotal = new ExpenseBillViewModel
            {
                DateTime = bill.Items.Count > 0 ? "за " + bill.DateTime.ToString("yyyy.MM.dd") : string.Empty,
                Subtotal = bill.Cost.ToString(culture),
                Subtotals = bill.Items.Select(x => new ExpenseItemViewModel
                {
                    Category = x.Category,
                    Product = x.Product,
                    Price = x.Cost.ToString(culture)
                }).ToList()
            };

            return View(subtotal);
        }

        public async Task<ActionResult> CommitBill()
        {
            var bill = GetCurrentBill();
            if (bill.Items.Count == 0)
                return RedirectToAction("Add", new { noItems = true });

            await _expensesBillCommands.Create(bill);
            Session[CurrentBillSessionName] = null;

            return RedirectToAction("Index");
        }
    }
}
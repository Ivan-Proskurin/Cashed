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
        private static readonly string DateFormat = "yyyy.MM.dd";
        private static readonly string DateTimeFormat = "yyyy.MM.dd HH:mm";
        private static readonly string DateFormatToHours = "yyyy.MM.dd HH:00";

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

        private async Task<ExpensesBillsViewList> CreateExpensesBillsViewList(
            DateTime dateFrom, DateTime dateTo)
        {
            var bills = await _expensesBillQueries.GetFiltered(dateFrom, dateTo);
            var totals = _expensesBillQueries.GetTotals(bills);
            return new ExpensesBillsViewList()
            {
                Bills = bills.Select(x => new ExpensesBillListViewModel
                {
                    Id = x.Id,
                    DateTime = x.DateTime.ToString(DateTimeFormat),
                    Category = x.Category,
                    Cost = x.Cost
                }).ToList(),

                Totals = new ExpensesTotalsViewModel
                {
                    Caption = totals.Caption,
                    Total = totals.Total
                },

                Filter = new ExpensesListFilter
                {
                    DateFrom = dateFrom.ToString(DateTimeFormat),
                    DateTo = dateTo.ToString(DateTimeFormat)
                }
            };
        }

        public async Task<ActionResult> Index()
        {
            var model = await CreateExpensesBillsViewList(DateTime.Today, DateTime.Today.AddDays(1));
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Index(ExpensesBillsViewList model)
        {
            var culture = new CultureInfo("ru-ru");
            var dateFrom = DateTime.Today;
            var dateTo = DateTime.Today.AddDays(1);
            if (ModelState.IsValid)
            {
                try
                {
                    dateFrom = DateTime.ParseExact(model.Filter.DateFrom, DateTimeFormat, culture);
                    dateTo = DateTime.ParseExact(model.Filter.DateTo, DateTimeFormat, culture);
                    if (dateFrom >= dateTo)
                        throw new ArgumentException("Дата До не может быть меньше или равной дате От");
                }
                catch (FormatException)
                {
                    ModelState.AddModelError(string.Empty, "Одна из введенных дат имела неверный формат");
                }
                catch (ArgumentException ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            var model2 = await CreateExpensesBillsViewList(dateFrom, dateTo);
            return View(model2);
        }

        public async Task<ActionResult> Add(string datetime = null, string category = null, bool noItems = false)
        {
            return View(await CreateExpenseItemViewModel(-1, datetime, category, noItems));
        }

        private async Task<ExpenseItemViewModel> CreateExpenseItemViewModel(int id, string datetime, string category, bool noItems)
        {
            var model = new ExpenseItemViewModel
            {
                Id = id,
                DateTime = datetime ?? DateTime.Now.ToString(DateFormatToHours),
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
                DateTime = DateTime.ParseExact(viewModel.DateTime, DateTimeFormat, CultureInfo.InvariantCulture),
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

        private void SetCurrentBill(ExpenseBillModel bill)
        {
            Session[CurrentBillSessionName] = bill;
        }

        public ActionResult GetSubtotal()
        {
            var culture = new CultureInfo("ru-ru");
            var bill = GetCurrentBill();
            var subtotal = new ExpenseBillViewModel
            {
                DateTime = bill.Items.Count > 0 ? "за " + bill.DateTime.ToString(DateFormat) : string.Empty,
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

            if (bill.Id > 0)
                await _expensesBillCommands.Update(bill);
            else
                await _expensesBillCommands.Create(bill);

            Session[CurrentBillSessionName] = null;

            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Edit(int id)
        {
            var bill = await _expensesBillQueries.GetById(id);
            SetCurrentBill(bill);
            return View("Add", await CreateExpenseItemViewModel(id, null, null, false));
        }

        [HttpPost]
        public async Task<ActionResult> Edit(ExpenseItemViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var expenseItem = await ExpenseItemViewModelToModel(model);
                    var bill = GetCurrentBill();
                    bill.AddItem(expenseItem);

                    return RedirectToAction("Add", new { datetime = model.DateTime, category = model.Category });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            model.AvailCategories = await GetAllCategoriesNames();
            return View("Add", model);
        }
    }
}
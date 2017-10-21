using Cashed.View.Models;
using Logic.Cashed.Contract;
using Logic.Cashed.Contract.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Cashed.Extensions;
using Newtonsoft.Json;

namespace Cashed.View.Controllers
{
    public class ExpensesController : Controller
    {
        private static readonly string CurrentBillSessionName = "CurrentBill";
        private static readonly string DeferredBillSessionName = "DeferredBill";
        private static readonly string CurrentFilterSessionName = "CurrentFilter";

        private readonly IExpensesBillQueries _expensesBillQueries;
        private readonly ICategoriesQueries _categoriesQueries;
        private readonly IProductQueries _productQueries;
        private readonly IExpensesBillCommands _expensesBillCommands;
        private readonly IUserSettings _userSettings;

        public ExpensesController(IExpensesBillQueries expensesBillQueries,
            ICategoriesQueries categoriesQueries, IProductQueries productQueries,
            IExpensesBillCommands expensesBillCommands, IUserSettings userSettings)
        {
            _expensesBillQueries = expensesBillQueries;
            _categoriesQueries = categoriesQueries;
            _productQueries = productQueries;
            _expensesBillCommands = expensesBillCommands;
            _userSettings = userSettings;
        }

        private async Task<ExpensesBillsViewList> CreateExpensesBillsViewList(
            ExpensesListFilter filter, int page = 1)
        {
            var args = new PaginationArgs
            {
                PageNumber = page,
                ItemsPerPage = _userSettings.ItemsPerPage
            };
            var bills = await _expensesBillQueries.GetFiltered(filter.DateFrom, filter.DateTo, args);
            var totals = _expensesBillQueries.GetTotals(bills.List);
            return new ExpensesBillsViewList
            {
                Bills = bills.List.Select(x => new ExpensesBillListViewModel
                {
                    Id = x.Id,
                    DateTime = x.DateTime.ToStandardString(),
                    Category = x.Category,
                    Cost = x.Cost
                }).ToList(),

                Totals = new ExpensesTotalsViewModel
                {
                    Caption = totals.Caption,
                    Total = totals.Total
                },

                Filter = new ExpensesListFilterViewModel
                {
                    DateFrom = filter.DateFrom.ToStandardString(),
                    DateTo = filter.DateTo.ToStandardString()
                },

                Pagination = bills.Pagination
            };
        }

        private ExpensesListFilter GetCurrentFilter()
        {
            return Session[CurrentFilterSessionName] as ExpensesListFilter;
        }

        private void SetCurrentFilter(ExpensesListFilter filter)
        {
            Session[CurrentFilterSessionName] = filter;
        }

        public async Task<ActionResult> Index(int page = 1)
        {
            var filter = GetCurrentFilter();
            if (filter == null)
            {
                filter = new ExpensesListFilter(DateTime.Today.StartOfTheWeek(), DateTime.Today.EndOfTheWeek());
                SetCurrentFilter(filter);
            }
            var model = await CreateExpensesBillsViewList(filter, page);
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Index(ExpensesBillsViewList model)
        {
            var dateFrom = DateTime.Today;
            var dateTo = DateTime.Today.AddDays(1);
            if (ModelState.IsValid)
            {
                try
                {
                    dateFrom = model.Filter.DateFrom.ParseDtFromStandardString();
                    dateTo = model.Filter.DateTo.ParseDtFromStandardString();
                    if (dateFrom >= dateTo)
                        throw new ArgumentException("Дата До не может быть меньше или равной дате От");
                }
                catch (FormatException)
                {
                    ModelState.AddModelError(string.Empty, @"Одна из введенных дат имела неверный формат");
                }
                catch (ArgumentException ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            var filter = new ExpensesListFilter(dateFrom, dateTo);
            SetCurrentFilter(filter);
            var model2 = await CreateExpensesBillsViewList(filter);
            return View(model2);
        }

        public async Task<ActionResult> Add(string datetime = null, string category = null, 
            bool noItems = false, bool newBill = true)
        {
            if (newBill) SetCurrentBill(null);
            return View(await CreateExpenseItemViewModel(-1, datetime, category, noItems));
        }

        private async Task<ExpenseItemViewModel> CreateExpenseItemViewModel(
            int id, string datetime, string category, bool noItems)
        {
            var model = new ExpenseItemViewModel
            {
                Id = id,
                DateTime = datetime ?? DateTime.Now.ToStandardString(false),
                Category = category,
                NoItems = noItems,
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
                    var bill = GetOrCreateCurrentBill();
                    bill.AddItem(expenseItem);
                    return RedirectToAction("Add", new {datetime = model.DateTime, category = model.Category, newBill = false});
                }
                catch (FormatException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            model.AvailCategories = await GetAllCategoriesNames();
            return View(model);
        }

        private async Task<ExpenseItemModel> ExpenseItemViewModelToModel(ExpenseItemViewModel viewModel)
        {
            var category = await _categoriesQueries.GetByName(viewModel.Category);
            var product = await _productQueries.GetByName(viewModel.Product);
            var model = new ExpenseItemModel
            {
                Id = -1,
                DateTime = viewModel.DateTime.ParseDtFromStandardString(),
                CategoryId = category.Id,
                Category = category.Name,
                ProductId = product.Id,
                Product = product.Name,
                Cost = viewModel.Price.ParseMoneyInvariant(),
                Quantity = string.IsNullOrWhiteSpace(viewModel.Quantity) ?
                    1 : viewModel.Quantity.ParseMoneyInvariant(),
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
            return Session[CurrentBillSessionName] as ExpenseBillModel;
        }

        public ExpenseBillModel GetCurrentBillOrThrowException()
        {
            var bill = GetCurrentBill();
            if (bill == null)
                throw new InvalidOperationException("Не найден текущий счет в текущей сессии");
            return bill;
        }

        private ExpenseBillModel GetOrCreateCurrentBill()
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
            var bill = GetOrCreateCurrentBill();
            var items = bill.GetItems();
            var subtotal = new ExpenseBillViewModel
            {
                DateTime = items.Count > 0 ? "за " + bill.DateTime.ToStandardDateStr() : string.Empty,
                Subtotal = bill.Cost.ToString(culture),
                Subtotals = items.Select(x => new ExpenseItemViewModel
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
            if (bill == null || bill.Items.Count == 0)
                return RedirectToAction("Add", new { noItems = true });

            if (bill.Id > 0)
                await _expensesBillCommands.Update(bill);
            else
                await _expensesBillCommands.Create(bill);

            SetCurrentBill(null);

            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Edit(int id)
        {
            var bill = GetCurrentBill();
            if (bill == null || bill.Id != id)
            {
                bill = await _expensesBillQueries.GetById(id);
                SetCurrentBill(bill);
            }
            return View("Add", await CreateExpenseItemViewModel(
                id, bill.DateTime.ToStandardString(), null, false));
        }

        [HttpPost]
        public async Task<ActionResult> Edit(ExpenseItemViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var expenseItem = await ExpenseItemViewModelToModel(model);
                    var bill = GetCurrentBillOrThrowException();
                    bill.AddItem(expenseItem);

                    return RedirectToAction("Add", new { datetime = model.DateTime, category = model.Category, newBill = false });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            model.AvailCategories = await GetAllCategoriesNames();
            return View("Add", model);
        }

        public ActionResult EditItems(bool deferBill = true)
        {
            var bill = GetCurrentBillOrThrowException();
            if (!deferBill) return View(bill);
            Session[DeferredBillSessionName] = bill;
            var clone = bill.Clone();
            SetCurrentBill(clone);
            return View(clone);
        }

        [HttpPost]
        public ActionResult EditItems(ExpenseBillModel model)
        {
            var bill = GetCurrentBill();
            if (bill != null)
            {
                Session[DeferredBillSessionName] = null;
                return RedirectToAction("Edit", new { id = bill.Id });
            }
            return RedirectToAction("Index");
        }

        public JsonResult DeleteItems(string itemIndicies)
        {
            var bill = GetCurrentBillOrThrowException();
            var indicies = JsonConvert.DeserializeObject<int[]>(itemIndicies);
            bill.DeleteItems(indicies);
            return Json(indicies, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CancelEditItems()
        {
            Session[CurrentBillSessionName] = Session[DeferredBillSessionName];
            var bill = GetCurrentBill();
            return bill != null ? RedirectToAction("Edit", new { id = bill.Id }) : RedirectToAction("Index");
        }

        public async Task<ActionResult> EditExpenseItem(int id)
        {
            var bill = GetCurrentBillOrThrowException();
            var item = bill.GetItem(id);
            return View(new ExpenseItemViewModel
            {
                Id = id,
                DateTime = item.DateTime.ToStandardString(),
                Category = item.Category,
                Product =  item.Product,
                Quantity = item.Quantity.ToStandardString(),
                Comment = item.Comment,
                Price = item.Cost.ToStandardString(),
                AvailCategories = await GetAllCategoriesNames()
            });
        }

        [HttpPost]
        public async Task<ActionResult> EditExpenseItem(ExpenseItemViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var item = await ExpenseItemViewModelToModel(model);
                    var bill = GetCurrentBillOrThrowException();
                    item.Id = model.Id;
                    item.IsModified = true;
                    bill.SetItem(item);
                    return RedirectToAction("EditItems", new {deferBill = false});
                }
                catch (FormatException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            model.AvailCategories = await GetAllCategoriesNames();
            return View(model);
        }

        public ActionResult CancelEditExpenseItem()
        {
            return RedirectToAction("EditItems", new {deferBill = false});
        }
    }
}
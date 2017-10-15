using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Cashed.View.Models;
using Logic.Cashed.Contract;
using Logic.Cashed.Contract.Models;
using Cashed.Extensions;
using System.Globalization;

namespace Cashed.View.Controllers
{
    public class IncomesController : Controller
    {
        private readonly IIncomeTypeQueries _incomeTypeQueries;
        private readonly IIncomeTypeCommands _incomeTypeCommands;
        private readonly IIncomeItemCommands _incomeItemCommands;

        public IncomesController(IIncomeTypeQueries incomeTypeQueries, IIncomeTypeCommands incomeTypeCommands,
            IIncomeItemCommands incomeItemCommands)
        {
            _incomeTypeQueries = incomeTypeQueries;
            _incomeTypeCommands = incomeTypeCommands;
            _incomeItemCommands = incomeItemCommands;
        }

        private async Task<IncomeTypesListViewModel> CreateModelFiltered(DateTime dateFrom, DateTime dateTo)
        {
            var types = await _incomeTypeQueries.GetFiltered(dateFrom, dateTo);
            var viewModel = new IncomeTypesListViewModel
            {
                List = types,
                Filter = new IncomesListFilter
                {
                    DateFrom = dateFrom.ToStandardString(),
                    DateTo = dateTo.ToStandardString()
                },
                Total = new TotalsInfoModel
                {
                    Caption = $"Итого за с {dateFrom.ToStandardDateStr()} по {dateTo.ToStandardDateStr()}:",
                    Total = types.Sum(x => x.SumTotal)
                }
            };
            return viewModel;
        }

        public async Task<ActionResult> Index()
        {
            var dateFrom = DateTime.Now.StartOfTheMonth();
            var dateTo = DateTime.Now.EndOfTheMonth();
            return View(await CreateModelFiltered(dateFrom, dateTo));
        }

        [HttpPost]
        public async Task<ActionResult> Index(IncomeTypesListViewModel model)
        {
            var dateFrom = DateTime.Today.StartOfTheMonth();
            var dateTo = DateTime.Today.EndOfTheMonth();
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
                    ModelState.AddModelError(string.Empty, "Одна из введенных дат имела неверный формат");
                }
                catch (ArgumentException ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            var model2 = await CreateModelFiltered(dateFrom, dateTo);
            return View(model2);
        }

        public ActionResult AddIncomeType()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> AddIncomeType(EditIncomeTypeViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var other = await _incomeTypeQueries.GetByName(model.Name);
                    if (other != null && other.Id != model.Id)
                        throw new ArgumentException($"Статья доходов с таким названием \"{model.Name}\" уже есть");

                    await _incomeTypeCommands.Update(new IncomeTypeModel
                    {
                        Id = -1,
                        Name = model.Name
                    });
                    return RedirectToAction("Index");
                }
                catch (ArgumentException ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(model);
        }

        public async Task<ActionResult> EditIncomeType(int id)
        {
            var model = await _incomeTypeQueries.GetById(id);
            if (model == null)
                RedirectToAction("Index");
            var viewModel = new EditIncomeTypeViewModel
            {
                Id = model.Id,
                Name = model.Name
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> EditIncomeType(EditIncomeTypeViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var other = await _incomeTypeQueries.GetByName(model.Name);
                    if (other != null && other.Id != model.Id)
                        throw new ArgumentException($"Статья доходов с таким названием \"{model.Name}\" уже есть");

                    await _incomeTypeCommands.Update(new IncomeTypeModel
                    {
                        Id = model.Id,
                        Name = model.Name
                    });
                    return RedirectToAction("Index");
                }
                catch (ArgumentException ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(model);
        }

        public async Task<ActionResult> AddIncome()
        {
            var types = await _incomeTypeQueries.GetAll();
            return View(new AddIncomeViewModel { IncomeTypes = types});
        }

        [HttpPost]
        public async Task<ActionResult> AddIncome(AddIncomeViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var type = await _incomeTypeQueries.GetByName(viewModel.IncomeType);
                    if (type == null)
                        throw new ArgumentException("Не найдено статьи расходов по имени.");
                    var dateTime = viewModel.DateTime.ParseDtFromStandardString();
                    var total = decimal.Parse(viewModel.Total.Replace(',', '.'), CultureInfo.InvariantCulture);

                    var model = new IncomeItemModel
                    {
                        Id = -1,
                        DateTime = dateTime,
                        IncomeTypeId = type.Id,
                        Total = total
                    };
                    await _incomeItemCommands.Update(model);

                    return RedirectToAction("Index");
                }
                catch (FormatException)
                {
                    ModelState.AddModelError(string.Empty, "Введенная строка имела неверный формат");
                }
            }
            viewModel.IncomeTypes = await _incomeTypeQueries.GetAll();
            return View(viewModel);
        }
    }
}
using Cashed.View.Models;
using Logic.Cashed.Contract;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Cashed.View.Controllers
{
    public class ExpensesController : Controller
    {
        private readonly IExpensesBillQueries _expensesBillQueries;

        public ExpensesController(IExpensesBillQueries expensesBillQueries)
        {
            _expensesBillQueries = expensesBillQueries;
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

        public ActionResult Add()
        {
            return View();
        }
    }
}
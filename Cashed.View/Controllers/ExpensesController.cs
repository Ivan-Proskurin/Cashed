using System.Web.Mvc;

namespace Cashed.View.Controllers
{
    public class ExpensesController : Controller
    {
        // GET: Expenses
        public ActionResult Index()
        {
            return View();
        }
    }
}
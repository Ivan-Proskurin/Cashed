using Cashed.View.Models;
using Logic.Cashed.Contract;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Cashed.View.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoriesQueries _categoriesQueries;

        public CategoryController(ICategoriesQueries categoriesQueries)
        {
            _categoriesQueries = categoriesQueries;
        }

        public async Task<ActionResult> Index()
        {
            var cats = await _categoriesQueries.GetAllCategories();
            return View(cats);
        }

        public ActionResult Add()
        {
            return View(new AddCategoryViewModel());
        }

        [HttpPost]
        public ActionResult Add(AddCategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }
            else
                return View(model);
        }
    }
}
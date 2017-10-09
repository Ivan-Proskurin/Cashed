using Cashed.View.Models;
using Logic.Cashed.Contract;
using Logic.Cashed.Contract.Models;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Cashed.View.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoriesQueries _categoriesQueries;
        private readonly ICategoriesCommands _categoriesCommands;

        public CategoryController(ICategoriesQueries categoriesQueries, 
            ICategoriesCommands categoriesCommands)
        {
            _categoriesQueries = categoriesQueries;
            _categoriesCommands = categoriesCommands;
        }

        public async Task<ActionResult> Index()
        {
            var cats = await _categoriesQueries.GetAll();
            return View(cats);
        }

        public ActionResult Add()
        {
            return View(new AddCategoryViewModel());
        }

        [HttpPost]
        public async Task<ActionResult> Add(AddCategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _categoriesCommands.Update(new CategoryModel
                    {
                        Id = -1,
                        Name = model.Name
                    });
                    return RedirectToAction("Index");
                }
                catch (ArgumentException exc)
                {
                    ModelState.AddModelError(string.Empty, exc.Message);
                    return View(model);
                }
            }
            else
                return View(model);
        }
    }
}
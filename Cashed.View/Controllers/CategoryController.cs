using Cashed.View.Models;
using Logic.Cashed.Contract;
using Logic.Cashed.Contract.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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

        public async Task<JsonResult> Delete(string idList)
        {
            var ids = JsonConvert.DeserializeObject<int[]>(idList);
            var deletedIds = new List<int>();
            foreach (var id in ids)
            {
                try
                {
                    await _categoriesCommands.Delete(id);
                    deletedIds.Add(id);
                }
                catch (ArgumentException)
                {

                }
            }
            return Json(deletedIds.ToArray(), JsonRequestBehavior.AllowGet);
        }

        private async void DeleteCategory(int id)
        {
            await _categoriesCommands.Delete(id);
        }

        public async Task<ActionResult> Edit(int id)
        {
            var category = await _categoriesQueries.GetById(id);
            if (category == null)
                throw new ArgumentException($"Нет категории с идентификатором {id}");
            return View(new EditCategoryViewModel
            {
                Id = category.Id,
                Name = category.Name
            });
        }

        [HttpPost]
        public async Task<ActionResult> Edit(EditCategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _categoriesCommands.Update(new CategoryModel
                    {
                        Id = model.Id,
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
            return View(model);
        }
    }
}
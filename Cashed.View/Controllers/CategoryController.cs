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
        private readonly IUserSettings _userSettings;

        public CategoryController(ICategoriesQueries categoriesQueries, 
            ICategoriesCommands categoriesCommands, IUserSettings userSettings)
        {
            _categoriesQueries = categoriesQueries;
            _categoriesCommands = categoriesCommands;
            _userSettings = userSettings;
        }

        public async Task<ActionResult> Index(int page = 1)
        {
            var viewModel = new CategoryListViewModel
            {
                List = await _categoriesQueries.GetList(new GetModelListArgs
                {
                    ItemsPerPage = _userSettings.ItemsPerPage,
                    PageNumber = page,
                    IncludeDeleted = false
                })
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> Add(CategoryListViewModel model)
        {
            if (ModelState.IsValid)
            try
            {
                var categoryName = model.Category;
                var category = await _categoriesQueries.GetByName(categoryName);
                if (category != null)
                    throw new ArgumentException($"Категория с названием \"{categoryName}\" уже есть");
                var cat = new CategoryModel
                {
                    Id = -1,
                    Name = categoryName
                };
                await _categoriesCommands.Update(cat);
                return RedirectToAction("Index", new { page = -1 });
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            model.List = await _categoriesQueries.GetList(new GetModelListArgs
            {
                ItemsPerPage = _userSettings.ItemsPerPage,
                PageNumber = 1,
                IncludeDeleted = false
            });
            return View("Index", model);
        }

        public async Task<JsonResult> AddCategory(string categoryName)
        {
            try
            {
                var category = await _categoriesQueries.GetByName(categoryName);
                if (category != null)
                    throw new ArgumentException($"Категория с названием \"{categoryName}\" уже есть");
                var model = new CategoryModel
                {
                    Id = -1,
                    Name = categoryName
                };
                model = await _categoriesCommands.Update(model);
                return Json(new { Model = model, Status = true }, JsonRequestBehavior.AllowGet);
            }
            catch (ArgumentException ex)
            {
                return Json(new { Status = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<JsonResult> Delete(string idList, bool onlyMark = true)
        {
            var ids = JsonConvert.DeserializeObject<int[]>(idList);
            var deletedIds = new List<int>();
            foreach (var id in ids)
            {
                try
                {
                    await _categoriesCommands.Delete(id, onlyMark);
                    deletedIds.Add(id);
                }
                catch (ArgumentException)
                {

                }
            }
            return Json(deletedIds.ToArray(), JsonRequestBehavior.AllowGet);
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Cashed.Logic.Contract;
using Cashed.Logic.Contract.Models;
using Cashed.View.Models;
using Newtonsoft.Json;

namespace Cashed.View.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ICategoriesQueries _categoriesQueries;
        private readonly IProductQueries _productQueries;
        private readonly IProductCommands _productCommands;

        public ProductsController(ICategoriesQueries categoriesQueries, IProductQueries productQueries,
            IProductCommands productCommands)
        {
            _categoriesQueries = categoriesQueries;
            _productQueries = productQueries;
            _productCommands = productCommands;
        }

        public async Task<ActionResult> Index(string categoryName = null)
        {
            var categories = await _categoriesQueries.GetAll();
            var viewModel = new ProductListViewModel
            {
                CategoryName = categoryName ?? categories.FirstOrDefault()?.Name,
                Categories = categories
            };
            return View(viewModel);
        }

        public async Task<JsonResult> GetProducts(string categoryName)
        {
            var category = await _categoriesQueries.GetByName(categoryName);
            if (category == null) return Json(new List<ProductModel>(), JsonRequestBehavior.AllowGet);
            var products = await _productQueries.GetCategoryProducts(category.Id);
            return Json(products, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> AddProduct(string categoryName, string productName)
        {
            try
            {
                var category = await _categoriesQueries.GetByName(categoryName);
                if (category == null)
                    throw new ArgumentException($"Нет категории с названием {categoryName}");
                var product = await _productQueries.GetByName(productName);
                if (product != null && product.CategoryId == category.Id)
                    throw new ArgumentException($"Продукт с таким названием уже есть ({productName})");
                var model = await _productCommands.AddProductToCategory(category.Id, productName);
                return Json(new {Model = model, Status = true}, JsonRequestBehavior.AllowGet);
            }
            catch (ArgumentException ex)
            {
                return Json(new {Status = false, ex.Message}, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<JsonResult> Delete(string idList, bool onlyMark = true)
        {
            var ids = JsonConvert.DeserializeObject<int[]>(idList);
            var deletedIds = await _productCommands.GroupDeletion(ids, onlyMark);
            return Json(deletedIds.ToArray(), JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> Edit(int id)
        {
            var model = await _productQueries.GetById(id);
            var category = await _categoriesQueries.GetById(model.CategoryId);
            var categories = await _categoriesQueries.GetAll();
            return View(new EditProductViewModel
            {
                Id = model.Id,
                Name = model.Name,
                CategoryId = model.CategoryId,
                CategoryName = category.Name,
                Categories = categories
            });
        }

        [HttpPost]
        public async Task<ActionResult> Edit(EditProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var category = await _categoriesQueries.GetByName(model.CategoryName);
                    if (category == null)
                        throw new ArgumentException($"Нет категории с названием \"{model.CategoryName}\"");

                    if (category.Id != model.CategoryId)
                    {
                        var products = await _productQueries.GetCategoryProducts(category.Id);
                        if (products.FirstOrDefault(
                                x => string.Equals(x.Name, model.Name, StringComparison.CurrentCultureIgnoreCase)) !=
                            null)
                        {
                            throw new ArgumentException($"В выбранной категории уже есть продукт с названием \"{model.Name}\"");
                        }
                    }

                    await _productCommands.Update(new ProductModel
                    {
                        Id = model.Id,
                        Name = model.Name,
                        CategoryId = category.Id
                    });
                    return RedirectToAction("Index", new { categoryName = category.Name });
                }
                catch (ArgumentException exc)
                {
                    ModelState.AddModelError(string.Empty, exc.Message);
                }
            }
            model.Categories = await _categoriesQueries.GetAll();
            return View(model);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using Cashed.View.Models;
using Logic.Cashed.Contract;
using Logic.Cashed.Contract.Models;
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

        public async Task<ActionResult> Index()
        {
            var viewModel = new ProductListViewModel
            {
                Categories = await _categoriesQueries.GetAll()
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
                return Json(new {Status = false, Message = ex.Message}, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<JsonResult> Delete(string idList)
        {
            var ids = JsonConvert.DeserializeObject<int[]>(idList);
            var deletedIds = await _productCommands.GroupDeletion(ids);
            return Json(deletedIds.ToArray(), JsonRequestBehavior.AllowGet);
        }
    }
}
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using Cashed.View.Models;
using Logic.Cashed.Contract;
using Logic.Cashed.Contract.Models;

namespace Cashed.View.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ICategoriesQueries _categoriesQueries;
        private readonly IProductQueries _productQueries;

        public ProductsController(ICategoriesQueries categoriesQueries, IProductQueries productQueries)
        {
            _categoriesQueries = categoriesQueries;
            _productQueries = productQueries;
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
    }
}
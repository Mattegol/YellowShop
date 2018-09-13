using System.Linq;
using System.Web.Mvc;
using YellowShop.Models;

namespace YellowShop.Controllers
{
    public class ShopController : Controller
    {
        private readonly ApplicationDbContext _context = new ApplicationDbContext();
        private const int PageSize = 3;

        // GET: Shop
        public ActionResult Index(int page = 1, int categoryId = 0, string searchString = "")
        {
            return View(GetModel(page, categoryId, searchString));
        }

        [HttpPost]
        public ActionResult Index(ProductsModel productsModel)
        {
            const int page = 1;
            var categoryId = productsModel.CategoryId;
            var searchString = productsModel.SearchString;

            return View(GetModel(page, categoryId, searchString));
        }

        private ProductsModel GetModel(int page, int categoryId, string searchString)
        {
            var data = _context.Products.Select(p => p)
                .Where(p => categoryId == 0 || p.CategoryId == categoryId)
                .Where(p => string.IsNullOrEmpty(searchString) || p.Description.Contains(searchString))
                .OrderBy(p => p.ProductName)
                .Skip((page - 1) * PageSize)
                .Take(PageSize);

            var model = new ProductsModel
            {
                Products = data,
                Pagination = new PaginationModel
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = categoryId == 0 ? 
                        _context.Products.Count() :
                        _context.Products
                            .Select(p => p)
                            .Where(p => p.CategoryId == categoryId)
                            .Count(p => p.Description.Contains(searchString))
                },
                CategoryId = categoryId
            };

            return model;
        }
    }
}
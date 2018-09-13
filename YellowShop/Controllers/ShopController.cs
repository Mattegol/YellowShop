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
        public ActionResult Index(int page = 1, int categoryId = 0)
        {
            return View(GetModel(page, categoryId));
        }

        [HttpPost]
        public ActionResult Index(ProductsModel productsModel)
        {
            const int page = 1;
            var categoryId = productsModel.CategoryId;

            return View(GetModel(page, categoryId));
        }

        private ProductsModel GetModel(int page, int categoryId)
        {
            var data = _context.Products.Select(p => p)
                .Where(p => categoryId == 0 || p.CategoryId == categoryId)
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
                    TotalItems = categoryId == 0 ? _context.Products.Count() :
                        _context.Products.Select(p => p).Count(p => p.CategoryId == categoryId)
                },
                CategoryId = categoryId
            };

            return model;
        }
    }
}
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
        public ActionResult Index(int page = 1)
        {
            var data = _context.Products.Select(p => p)
                    .OrderBy(p => p.ProductName)
                    .Skip((page - 1) * PageSize).Take(PageSize);

            var model = new ProductsModel
            {
                Products = data,
                Pagination = new PaginationModel
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = _context.Products.ToList().Count()
                }
            };

            return View(model);
        }
    }
}
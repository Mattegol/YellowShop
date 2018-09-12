using System.Linq;
using System.Web.Mvc;
using YellowShop.Models;

namespace YellowShop.Controllers
{
    public class ShopController : Controller
    {
        private readonly ApplicationDbContext _context = new ApplicationDbContext();

        // GET: Shop
        public ActionResult Index()
        {
            var data = _context.Products.Select(p => p).OrderBy(p => p.ProductName);

            return View(data);
        }
    }
}
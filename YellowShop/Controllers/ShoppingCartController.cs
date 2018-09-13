using System.Linq;
using System.Web.Mvc;
using YellowShop.Models;

namespace YellowShop.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly ApplicationDbContext _context = new ApplicationDbContext();

        public ShoppingCartController()
        {

        }

        // GET: ShoppingCart
        public ActionResult Index(string returnUrl)
        {
            return View(new ShoppingCartViewModel
            {
                Cart = GetCart(),
                ReturnUrl = returnUrl
            });
        }

        public RedirectToRouteResult AddToCart(int id, string returnUrl)
        {
            var product = _context.Products.SingleOrDefault(p => p.Id == id);

            if (product != null)
            {
                GetCart().AddItem(product, 1);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        public RedirectToRouteResult RemoveFromCart(int id, string returnUrl)
        {
            GetCart().RemoveItem(id);

            return RedirectToAction("Index", new { returnUrl });
        }

        public PartialViewResult CartWidget()
        {
            return PartialView(GetCart());
        }


        private ShoppingCartModel GetCart()
        {
            var cart = (ShoppingCartModel)Session["Cart"];
            if (cart != null) return cart;

            cart = new ShoppingCartModel();
            Session["Cart"] = cart;

            return cart;
        }
    }
}
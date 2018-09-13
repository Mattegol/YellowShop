using System;
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

        public ViewResult ShippingInfo()
        {
            return View(new ShippingInfo());
        }

        [HttpPost]
        public ActionResult ShippingInfo(ShippingInfo shippingInfo)
        {
            if (!ModelState.IsValid)
            {
                return View(shippingInfo);
            }

            ShoppingCartModel cart = GetCart();
            cart.ShippingInfo = shippingInfo;

            return RedirectToAction("BillingInfo");
        }

        public ViewResult BillingInfo()
        {
            return View(new BillingInfo());
        }

        [HttpPost]
        public ViewResult BillingInfo(BillingInfo billingInfo)
        {
            if (!ModelState.IsValid)
            {
                return View(billingInfo);
            }

            ShoppingCartModel cart = GetCart();
            cart.BillingInfo = billingInfo;
            ProcessOrder(cart);
            cart.Clear();

            return View("OrderComplete");
        }

        private void ProcessOrder(ShoppingCartModel cart)
        {
            // to do: we need a login for our customer, until then we will create one every time.
            var customer = new Customer
            {
                FirstName = cart.BillingInfo.FirstName,
                LastName = cart.BillingInfo.LastName,
                BillingAddress = cart.BillingInfo.Address,
                BillingCity = cart.BillingInfo.City,
                BillingState = cart.BillingInfo.State,
                BillingPostalCode = cart.BillingInfo.Zip,
            };
            _context.Customers.Add(customer);
            _context.SaveChanges();

            var order = new Order
            {
                CustomerId = customer.Id,
                OrderDate = DateTime.Now,
                ShippingAddress = cart.ShippingInfo.Address,
                ShippingCity = cart.ShippingInfo.City,
                ShippingState = cart.ShippingInfo.State,
                ShippingPostalCode = cart.ShippingInfo.Zip
            };
            _context.Orders.Add(order);
            _context.SaveChanges();

            foreach (ShoppingCartItemModel item in cart.Items)
            {
                var orderItem = new OrderItem
                {
                    OrderId = order.Id,
                    ProductId = item.Product.Id,
                    Quantity = item.Quantity
                };
                _context.OrderItems.Add(orderItem);
            }
            _context.SaveChanges();
        }
    }
}
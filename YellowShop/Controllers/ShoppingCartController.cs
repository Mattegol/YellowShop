using Microsoft.AspNet.Identity;
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

        [Authorize]
        public ViewResult ShippingInfo()
        {
            var userId = User.Identity.GetUserId();

            var shippingInfo = new ShippingInfo();
            var customer = _context.Customers.SingleOrDefault(c => c.UserId == userId);

            if (customer == null) return View(shippingInfo);

            shippingInfo.Address = customer.Address;
            shippingInfo.City = customer.City;
            shippingInfo.State = customer.State;
            shippingInfo.Zip = customer.PostalCode;

            return View(shippingInfo);
        }

        [Authorize]
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

        [Authorize]
        public ViewResult BillingInfo()
        {
            var userId = User.Identity.GetUserId();

            var billingInfo = new BillingInfo();
            var customer = _context.Customers.SingleOrDefault(c => c.UserId == userId);

            if (customer == null) return View(billingInfo);

            billingInfo.FirstName = customer.FirstName;
            billingInfo.LastName = customer.LastName;
            billingInfo.Address = customer.Address;
            billingInfo.City = customer.City;
            billingInfo.State = customer.State;
            billingInfo.Zip = customer.PostalCode;


            return View(billingInfo);
        }

        [Authorize]
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

        [Authorize]
        private void ProcessOrder(ShoppingCartModel cart)
        {
            var userId = User.Identity.GetUserId();

            var customer = _context.Customers.SingleOrDefault(c => c.UserId == userId);

            if (customer != null)
            {
                customer.FirstName = cart.BillingInfo.FirstName;
                customer.LastName = cart.BillingInfo.LastName;
                customer.BillingAddress = cart.BillingInfo.Address;
                customer.BillingCity = cart.BillingInfo.City;
                customer.BillingState = cart.BillingInfo.State;
                customer.BillingPostalCode = cart.BillingInfo.Zip;

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
            }
            _context.SaveChanges();
        }
    }
}
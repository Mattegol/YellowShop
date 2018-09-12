using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YellowShop.Models;

namespace YellowShop.Controllers
{
    public class ItemDetailController : Controller
    {
        private readonly ApplicationDbContext _context = new ApplicationDbContext();

        // GET: ItemDetail
        public ActionResult Index(int id)
        {
            var data = _context.Products.SingleOrDefault(p => p.Id == id);

            return View(data);
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace YellowShop.Models
{
    public class ProductsModel
    {
        public IEnumerable<Product> Products { get; set; }

        public PaginationModel Pagination { get; set; }

        public int CategoryId { get; set; }

        public string SearchString { get; set; }

        public SelectList Categories()
        {
            var context = new ApplicationDbContext();
            var categories = from c in context.Categories
                             orderby c.CategoryName
                             select new
                             {
                                 c.Id,
                                 c.CategoryName
                             };

            return new SelectList(categories, "Id", "CategoryName");
        }

    }
}
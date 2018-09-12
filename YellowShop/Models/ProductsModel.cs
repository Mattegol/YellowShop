using System.Collections.Generic;

namespace YellowShop.Models
{
    public class ProductsModel
    {
        public IEnumerable<Product> Products { get; set; }

        public PaginationModel Pagination { get; set; }

    }
}
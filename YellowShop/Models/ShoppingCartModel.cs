using System.Collections.Generic;
using System.Linq;

namespace YellowShop.Models
{
    public class ShoppingCartModel
    {
        private readonly List<ShoppingCartItemModel> _items = new List<ShoppingCartItemModel>();

        public IEnumerable<ShoppingCartItemModel> Items => _items;

        public void AddItem(Product product, int quantity)
        {
            var item = _items.SingleOrDefault(p => p.Product.Id == product.Id);

            if (item == null)
            {
                _items.Add(new ShoppingCartItemModel
                {
                    Product = product,
                    Quantity = quantity
                });
            }
            else
            {
                item.Quantity += quantity;
            }
        }

        public void RemoveItem(int id)
        {
            _items.RemoveAll(i => i.Product.Id == id);
        }

        public decimal GetCartTotal()
        {
            return _items.Sum(e => e.Product.Price * e.Quantity);
        }

        public void Clear()
        {
            _items.Clear();
        }
    }
}
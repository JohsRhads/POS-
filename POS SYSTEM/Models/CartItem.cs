using System;
using System.Collections.Generic;
using System.Text;

namespace POS_SYSTEM.Models
{
    public class CartItem
    {
        public Product Product;
        public int Quantity;

        public CartItem(Product Product, int Quantity)
        {
            this.Product = Product;
            this.Quantity = Quantity;
        }
    }
}

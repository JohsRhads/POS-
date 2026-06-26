    using System;
    using System.Collections.Generic;


    namespace POS_SYSTEM.Models
    {
        public class Cart
        {
            private List<CartItem> items = new List<CartItem> ();

            public void AddItem(Product product , int quantity)
            {
                var existingItem = items.Find(i => i.Product.Id == product.Id);

                if (existingItem != null)
                {
                    existingItem.Quantity += quantity;  // Increase quantity
                }
                else
                {
                    items.Add(new CartItem(product, quantity));  // Add new item
                }
            }
            public void RemoveItem(int productId)
            {
                var RemoveById = items.FirstOrDefault(i => i.Product.Id == productId);
                if (RemoveById == null)
                {
                    Console.WriteLine("ITEM ID NOT FOUND.");    
                }
                else
                {
                    Console.WriteLine($"ITEM {RemoveById.Product.Name} HAS REMOVED");
                    items.RemoveAll(b => b.Product.Id == productId);
                }
            }
            public decimal GetTotal()
            {
                decimal total = 0;
                foreach (var item in items)
                {
                    total += item.Product.Price * item.Quantity;
                }
                return total;
            }
            public void Clear()
            {
                items.Clear();
            }
            public int GetItemCount()
            {
                int total = 0;
                foreach(var item in items)
                {
                    total += item.Quantity;
                }
                return total;
            }
        public List<CartItem> GetItems()
        {
            return items;
        }

    }
    }
    
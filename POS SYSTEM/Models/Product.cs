using System;

namespace POS_SYSTEM.Models
{
    public class Product
    {
        private static int _nextId = 1;

        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public int? CategoryId { get; set; }        // FK — can be null
        public string? CategoryName { get; set; }   // From JOIN — read only

        // For SQL queries (no Id assignment)
        public Product() { }

        // For creating new products
        public Product(string name, decimal price, int stock, int? categoryId = null)
        {
            Id = _nextId++;
            Name = name;
            Price = price;
            Stock = stock;
            CategoryId = categoryId;
        }
    }
}
using System;
using System.Collections.Generic;
    

namespace POS_SYSTEM.Models
{
    public class Product
    {
        private static int _nextId = 1;  // ← Only Product manages this

        public int Id { get; set; }       // ← Others can read
        public string Name { get; set; }  // ← Others can read/write
        public decimal Price { get; set; }// ← Others can read/write
        public int Stock { get; set; }    // ← Others can read/write

        public Product(string name, decimal price, int stock)
        {
            Id = _nextId++;  // ← Private counter used here
            Name = name;
            Price = price;
            Stock = stock;
        }
    }
}
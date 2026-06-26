    using POS_SYSTEM.Models;
    using System;
    using System.Collections.Generic;
   

    namespace POS_SYSTEM.Repos
    {
        public  class ProductRepository
        {
            private List<Product> products = new List<Product>();

            public void AddProduct(Product product)
            {
                var IfProductExist = products.Any(p => p.Name == product.Name);

                if(IfProductExist)
                {
                    Console.WriteLine("THIS ITEM IS ALREADY EXIST.");
                    return;
                }
                products.Add(product);
            }   
            public List<Product> GetAll()
            {
                return products;
            }
            public Product GetById(int id)
            {
                
                return products.FirstOrDefault(p => p.Id == id);
            }
        public void Update(Product updatedProduct)
        {
            // Step 1: Find existing product by ID
            var existing = products.FirstOrDefault(p => p.Id == updatedProduct.Id);

            // Step 2: If not found, stop
            if (existing == null)
            {
                Console.WriteLine("Product not found.");
                return;
            }

            // Step 3: Update properties (no constructor needed)
            existing.Name = updatedProduct.Name;
            existing.Price = updatedProduct.Price;
            existing.Stock = updatedProduct.Stock;
        }
        public void Delete(int id)
        {
            int removed = products.RemoveAll(p => p.Id == id);

            if (removed > 0)
                Console.WriteLine("Product deleted.");
            else
                Console.WriteLine("Product not found.");
        }
    }
}

using POS_SYSTEM.Models;
using POS_SYSTEM.Repos;
using System;
using System.Collections.Generic;

namespace POS_SYSTEM.Services
{
    public class ProductService
    {
        private ProductRepository repos;
        
        public ProductService(ProductRepository repo)
        {
            repos = repo;
        }

        public void AddProduct(string name ,decimal  price , int stock)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("NAME CANNOT BE EMPTY");
                return;
                
            }
            if (price <= 0)
            {
                Console.WriteLine("PLEASE INPUT VALID PRICE");
                return;
            }
            if (stock <= 0)
            {
                Console.WriteLine("PLEASE INNPUT VALID STOCK");
                return;
            }
            Product product = new Product(name,price,stock);
            repos.AddProduct(product);
        }
        public List<Product> GetAllProducts()
        {
            var products = repos.GetAll();

            if (products == null || products.Count == 0)
            {
                Console.WriteLine("No products found.");
                return new List<Product>(); // Return empty list
            }

            return products;
        }
        public Product GetProductById(int id)
        {
            if(id <= 0)
            {
                Console.WriteLine("PLEASE ENTER A VALID ID");
                return null;
            }
            return repos.GetById(id);
        }
        public void UpdateProduct(int id, string name, decimal price, int stock)
        {
            // ✓ VALIDATION 1: Check if ID is valid
            if (id <= 0)
            {
                Console.WriteLine("INVALID ID. ID MUST BE GREATER THAN 0.");
                return;
            }

            // ✓ VALIDATION 2: Check if product exists in the repository
            var existingProduct = repos.GetById(id);
            if (existingProduct == null)
            {
                Console.WriteLine($"PRODUCT WITH ID {id} NOT FOUND.");
                return;
            }

            // ✓ VALIDATION 3: Check if name is empty or whitespace
            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("NAME CANNOT BE EMPTY");
                return;
            }

            // ✓ VALIDATION 4: Check if price is valid (greater than 0)
            if (price <= 0)
            {
                Console.WriteLine("PLEASE INPUT VALID PRICE (MUST BE GREATER THAN 0)");
                return;
            }

            // ✓ VALIDATION 5: Check if stock is valid (greater than or equal to 0)
            if (stock < 0)  // Note: Stock can be 0, but not negative
            {
                Console.WriteLine("PLEASE INPUT VALID STOCK (CANNOT BE NEGATIVE)");
                return;
            }

            // ✓ All validations passed - Create updated product
            Product updatedProduct = new Product(name, price, stock);
            updatedProduct.Id = id; // Set the ID to match existing product

            // ✓ Call repository to update
            repos.Update(updatedProduct);

            // ✓ Success message
            Console.WriteLine($"PRODUCT WITH ID {id} UPDATED SUCCESSFULLY!");
        }
        public void DeleteProduct(int id)
        {
            // ✓ VALIDATION 1: Check if ID is valid
            if (id <= 0)
            {
                Console.WriteLine("INVALID ID. ID MUST BE GREATER THAN 0.");
                return;
            }

            // ✓ VALIDATION 2: Check if product exists before deleting
            var existingProduct = repos.GetById(id);
            if (existingProduct == null)
            {
                Console.WriteLine($"PRODUCT WITH ID {id} NOT FOUND.");
                return;
            }

            // ✓ Call repository to delete
            repos.Delete(id);

            // ✓ Success message (optional, since your repo already has one)
            Console.WriteLine($"PRODUCT WITH ID {id} DELETED SUCCESSFULLY!");
        }
    }
}

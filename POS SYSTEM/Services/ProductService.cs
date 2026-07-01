using POS_SYSTEM.Models;
using POS_SYSTEM.Repos;
using System;
using System.Collections.Generic;

namespace POS_SYSTEM.Services
{
    public class ProductService
    {
        private ProductSqlRepository repo;

        public ProductService(ProductSqlRepository repo)
        {
            this.repo = repo;
        }

        public List<Product> GetAllProducts()
        {
            var products = repo.GetAll();

            if (products == null || products.Count == 0)
            {
                Console.WriteLine("No products found.");
                return new List<Product>();
            }

            return products;
        }

        public void AddProduct(string name, decimal price, int stock, int? categoryId = null)
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
            if (stock < 0)
            {
                Console.WriteLine("PLEASE INPUT VALID STOCK");
                return;
            }

            Product product = new Product(name, price, stock, categoryId);
            repo.Add(product);
            Console.WriteLine($"Product '{name}' added successfully.");
        }

        public Product GetById(int id)
        {
            var product = repo.GetById(id);

            if (product == null)
                Console.WriteLine("Product not found.");

            return product;
        }

        public void UpdateProduct(int id, string name, decimal price, int stock, int? categoryId = null)
        {
            var existingProduct = repo.GetById(id);

            if (existingProduct == null)
            {
                Console.WriteLine($"Product with ID {id} not found.");
                return;
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Name cannot be empty.");
                return;
            }

            if (price <= 0)
            {
                Console.WriteLine("Price must be greater than 0.");
                return;
            }

            if (stock < 0)
            {
                Console.WriteLine("Stock cannot be negative.");
                return;
            }

            existingProduct.Name = name;
            existingProduct.Price = price;
            existingProduct.Stock = stock;
            existingProduct.CategoryId = categoryId;

            repo.UpdateProduct(existingProduct);
            Console.WriteLine($"Product with ID {id} updated successfully.");
        }

        public void DeleteProduct(int id)
        {
            var existingProduct = repo.GetById(id);

            if (existingProduct == null)
            {
                Console.WriteLine($"Product with ID {id} not found.");
                return;
            }

            repo.DeleteProduct(id);
            Console.WriteLine($"Product with ID {id} deleted successfully.");
        }
        public List<Category> GetAllCategories()
        {
            return repo.GetAllCategories();
        }
    }
}
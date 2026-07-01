using POS_SYSTEM.Models;
using POS_SYSTEM.Repos;
using POS_SYSTEM.Services;
using POS_SYSTEM.Interfaces;
using System;

namespace POS_SYSTEM
{
    class Program
    {
        public static void Main(string[] args)
        {
            ProductSqlRepository sqlRepo = new ProductSqlRepository();
            ProductService productService = new ProductService(sqlRepo);
            Cart cart = new Cart();
            CheckoutService checkout = new CheckoutService(productService, cart);

            bool running = true;

            while (running)
            {
                Console.Clear();
                Console.WriteLine("════════════════════════════════");
                Console.WriteLine("         POS SYSTEM");
                Console.WriteLine("════════════════════════════════");
                Console.WriteLine("1. Add Product");
                Console.WriteLine("2. View All Products");
                Console.WriteLine("3. Update Product");
                Console.WriteLine("4. Delete Product");
                Console.WriteLine("5. Add to Cart");
                Console.WriteLine("6. View Cart");
                Console.WriteLine("7. Checkout (Cash)");
                Console.WriteLine("8. Exit");
                Console.WriteLine("════════════════════════════════");
                Console.Write("Enter your choice: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddProductFlow(productService);
                        break;
                    case "2":
                        ViewAllProducts(productService);
                        break;
                    case "3":
                        UpdateProductFlow(productService);
                        break;
                    case "4":
                        DeleteProductFlow(productService);
                        break;
                    case "5":
                        AddToCartFlow(checkout);
                        break;
                    case "6":
                        checkout.ViewCart();
                        break;
                    case "7":
                        CheckoutFlow(checkout);
                        break;
                    case "8":
                        running = false;
                        Console.WriteLine("Thank you for using POS System!");
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Try again.");
                        break;
                }

                if (running)
                {
                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey();
                }
            }
        }

        static void AddProductFlow(ProductService service)
        {
            Console.Clear();
            Console.WriteLine("=== ADD PRODUCT ===");

            // "Show available categories"
            // "Show available categories"
            Console.WriteLine("\nAvailable Categories:");
            var categories = service.GetAllCategories();
            foreach (var cat in categories)
            {
                Console.WriteLine($"  {cat.Id}. {cat.Name}");
            }
            Console.WriteLine();

            Console.Write("\nEnter product name: ");
            string name = Console.ReadLine();

            decimal price;
            while (true)
            {
                Console.Write("Enter price: ");
                if (decimal.TryParse(Console.ReadLine(), out price))
                    break;
                Console.WriteLine("Invalid price. Try again.");
            }

            int stock;
            while (true)
            {
                Console.Write("Enter stock: ");
                if (int.TryParse(Console.ReadLine(), out stock))
                    break;
                Console.WriteLine("Invalid stock. Try again.");
            }

            // "Ask for category ID"
            int? categoryId = null;
            Console.Write("Enter Category ID (or press Enter to skip): ");
            string catInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(catInput) && int.TryParse(catInput, out int catId))
            {
                categoryId = catId;
            }

            service.AddProduct(name, price, stock, categoryId);
        }

        static void ViewAllProducts(ProductService service)
        {
            Console.Clear();
            Console.WriteLine("=== ALL PRODUCTS ===");
            var products = service.GetAllProducts();

            if (products.Count == 0)
            {
                Console.WriteLine("No products found.");
                return;
            }

            // UPDATED: Added Category column
            Console.WriteLine($"{"ID",-5} {"Name",-20} {"Price",-10} {"Stock",-8} {"Category",-15}");
            Console.WriteLine(new string('-', 60));
            foreach (var p in products)
            {
                string category = p.CategoryName ?? "None";
                Console.WriteLine($"{p.Id,-5} {p.Name,-20} ₱{p.Price,-9:N2} {p.Stock,-8} {category,-15}");
            }
        }

        static void UpdateProductFlow(ProductService service)
        {

            Console.Clear();
            Console.WriteLine("=== UPDATE PRODUCT ===");

            int id;
            while (true)
            {
                Console.Write("Enter product ID to update: ");
                if (int.TryParse(Console.ReadLine(), out id))
                    break;
                Console.WriteLine("Invalid ID. Try again.");
            }

            Console.Write("Enter new name: ");
            string name = Console.ReadLine();

            decimal price;
            while (true)
            {
                Console.Write("Enter new price: ");
                if (decimal.TryParse(Console.ReadLine(), out price))
                    break;
                Console.WriteLine("Invalid price. Try again.");
            }

            int stock;
            while (true)
            {
                Console.Write("Enter new stock: ");
                if (int.TryParse(Console.ReadLine(), out stock))
                    break;
                Console.WriteLine("Invalid stock. Try again.");
            }
            Console.WriteLine("\nAvailable Categories:");
            var categories = service.GetAllCategories();
            foreach (var cat in categories)
            {
                Console.WriteLine($"  {cat.Id}. {cat.Name}");
            }
            // NEW: Ask for category ID
            int? categoryId = null;
            Console.Write("Enter new Category ID (or press Enter to skip): ");
            string catInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(catInput) && int.TryParse(catInput, out int catId))
            {
                categoryId = catId;
            }

            service.UpdateProduct(id, name, price, stock, categoryId);
        }

        static void DeleteProductFlow(ProductService service)
        {
            Console.Clear();
            Console.WriteLine("=== DELETE PRODUCT ===");

            int id;
            while (true)
            {
                Console.Write("Enter product ID to delete: ");
                if (int.TryParse(Console.ReadLine(), out id))
                    break;
                Console.WriteLine("Invalid ID. Try again.");
            }

            service.DeleteProduct(id);
        }

        static void AddToCartFlow(CheckoutService checkout)
        {
            Console.Clear();
            Console.WriteLine("=== ADD TO CART ===");

            int productId;
            while (true)
            {
                Console.Write("Enter product ID: ");
                if (int.TryParse(Console.ReadLine(), out productId))
                    break;
                Console.WriteLine("Invalid ID. Try again.");
            }

            int quantity;
            while (true)
            {
                Console.Write("Enter quantity: ");
                if (int.TryParse(Console.ReadLine(), out quantity))
                    break;
                Console.WriteLine("Invalid quantity. Try again.");
            }

            checkout.AddToCart(productId, quantity);
        }

        static void CheckoutFlow(CheckoutService checkout)
        {
            Console.Clear();
            Console.WriteLine("=== CHECKOUT ===");
                
            IPaymentProcessor payment = new CashPayment();
            checkout.Checkout(payment);
        }
    }

    public class CashPayment : IPaymentProcessor
    {
        public bool ProcessPayment(decimal amount)
        {
            Console.WriteLine($"\nAmount due: ₱{amount:N2}");
            Console.Write("Enter cash amount: ");

            decimal cash;
            while (!decimal.TryParse(Console.ReadLine(), out cash) || cash < amount)
            {
                Console.WriteLine("Insufficient amount. Try again.");
                Console.Write("Enter cash amount: ");
            }

            decimal change = cash - amount;
            Console.WriteLine($"Payment accepted. Change: ₱{change:N2}");
            return true;
        }
    }
}
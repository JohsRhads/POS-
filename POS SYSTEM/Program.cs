using POS_SYSTEM.Models;
using POS_SYSTEM.Repos;
using POS_SYSTEM.Services;
using POS_SYSTEM.Interfaces;
using System;

namespace POS_SYSTEM
{
    class Program
    {
        static void Main(string[] args)
        {
            ProductRepository repo = new ProductRepository();
            ProductService productService = new ProductService(repo);
            Cart cart = new Cart();
            CheckoutService checkout = new CheckoutService(productService, cart);

            productService.AddProduct("Laptop", 50000, 10);
            productService.AddProduct("Mouse", 500, 50);
            productService.AddProduct("Keyboard", 1500, 30);
            productService.AddProduct("Monitor", 8000, 15);

            bool running = true;

            while (running)
            {
                Console.Clear();
                Console.WriteLine("════════════════════════════════");
                Console.WriteLine("         POS SYSTEM");
                Console.WriteLine("════════════════════════════════");
                Console.WriteLine("1. Add Product");
                Console.WriteLine("2. View All Products");
                Console.WriteLine("3. Add to Cart");
                Console.WriteLine("4. View Cart");
                Console.WriteLine("5. Remove from Cart");
                Console.WriteLine("6. Checkout (Cash)");
                Console.WriteLine("7. Exit");
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
                        AddToCartFlow(checkout);
                        break;
                    case "4":
                        checkout.ViewCart();
                        break;
                    case "5":
                        RemoveFromCartFlow(checkout);
                        break;
                    case "6":
                        CheckoutFlow(checkout);
                        break;
                    case "7":
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

            Console.Write("Enter product name: ");
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

            service.AddProduct(name, price, stock);
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

            Console.WriteLine($"{"ID",-5} {"Name",-20} {"Price",-10} {"Stock",-10}");
            Console.WriteLine(new string('-', 45));
            foreach (var p in products)
            {
                Console.WriteLine($"{p.Id,-5} {p.Name,-20} ₱{p.Price,-9:N2} {p.Stock,-10}");
            }
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

        static void RemoveFromCartFlow(CheckoutService checkout)
        {
            Console.Clear();
            Console.WriteLine("=== REMOVE FROM CART ===");

            int productId;
            while (true)
            {
                Console.Write("Enter product ID to remove: ");
                if (int.TryParse(Console.ReadLine(), out productId))
                    break;
                Console.WriteLine("Invalid ID. Try again.");
            }

            checkout.RemoveFromCart(productId);
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
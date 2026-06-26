using POS_SYSTEM.Interfaces;
using POS_SYSTEM.Models;
using POS_SYSTEM.Services;
using System;


namespace POS_SYSTEM.Services
{
    public class CheckoutService
    {
        private ProductService _productService;
        private Cart _cart;

        public CheckoutService(ProductService productService, Cart cart)
        {
            _productService = productService;
            _cart = cart;
        }

        public void AddToCart(int productId, int quantity)
        {
            // Find the product
            var product = _productService.GetProductById(productId);

            // Check if product exists
            if (product == null)
            {
                Console.WriteLine("Product not found.");
                return;
            }

            // Check if enough stock
            if (quantity > product.Stock)
            {
                Console.WriteLine($"Not enough stock. Only {product.Stock} available.");
                return;
            }

            // Add to cart
            _cart.AddItem(product, quantity);
            Console.WriteLine($"Added {quantity} x {product.Name} to cart.");
        }

        public void RemoveFromCart(int productId)
        {
            _cart.RemoveItem(productId);
        }

        public void ViewCart()
        {
            var items = _cart.GetItems(); // You'll need to add this to Cart.cs

            if (items.Count == 0)
            {
                Console.WriteLine("Cart is empty.");
                return;
            }

            Console.WriteLine("\n=== YOUR CART ===");
            foreach (var item in items)
            {
                decimal subtotal = item.Product.Price * item.Quantity;
                Console.WriteLine($"{item.Product.Name} x{item.Quantity} = ₱{subtotal:N2}");
            }
            Console.WriteLine($"\nTotal: ₱{_cart.GetTotal():N2}");
        }

        public decimal GetCartTotal()
        {
            return _cart.GetTotal();
        }

        public bool Checkout(IPaymentProcessor processor)
        {
            var items = _cart.GetItems();

            if (items.Count == 0)
            {
                Console.WriteLine("Cart is empty.");
                return false;
            }

            decimal total = _cart.GetTotal();
            Console.WriteLine($"\nTotal amount: ₱{total:N2}");
            Console.WriteLine("Processing payment...");

            bool paymentSuccess = processor.ProcessPayment(total);

            if (!paymentSuccess)
            {
                Console.WriteLine("Payment declined.");
                return false;
            }

            // Reduce stock
            foreach (var item in items)
            {
                var product = _productService.GetProductById(item.Product.Id);
                if (product != null)
                {
                    product.Stock -= item.Quantity;
                }
            }

            _cart.Clear();
            Console.WriteLine("Payment successful! Thank you for your purchase.");
            return true;
        }
    }
}
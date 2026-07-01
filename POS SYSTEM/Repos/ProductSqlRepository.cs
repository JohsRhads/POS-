using Microsoft.Data.SqlClient;
using POS_SYSTEM.Models;
using System;
using System.Collections.Generic;

namespace POS_SYSTEM.Repos
{
    public class ProductSqlRepository
    {
        private readonly string _connectionString =
            "Server=.\\SQLEXPRESS;Database=POSSystem;Trusted_Connection=True;TrustServerCertificate=True;";

        // ==========================================
        // GET ALL — Now includes category name via JOIN
        // ==========================================
        public List<Product> GetAll()
        {
            var products = new List<Product>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // New way: Call the saved procedure by name
                using (SqlCommand command = new SqlCommand("GetAllProducts", connection))  // ← Procedure name
                {
                    // Tell C# this is a stored procedure, not raw SQL
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Product product = new Product(
                                reader.GetString(1),
                                reader.GetDecimal(2),
                                reader.GetInt32(3)
                            );
                            product.Id = reader.GetInt32(0);
                            if (!reader.IsDBNull(4))
                                product.CategoryName = reader.GetString(4);
                            products.Add(product);
                        }
                    }
                }
            }
            return products;
        }

        // ==========================================
        // ADD — UPDATED: Now includes CategoryId
        // ==========================================
        public void Add(Product product)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // "Call the saved AddProduct procedure"
                using (SqlCommand command = new SqlCommand("AddProduct", connection))
                {
                    // "Tell C# this is a stored procedure"
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    // "Pass the values to the procedure"
                    command.Parameters.AddWithValue("@Name", product.Name);
                    command.Parameters.AddWithValue("@Price", product.Price);
                    command.Parameters.AddWithValue("@Stock", product.Stock);
                    command.Parameters.AddWithValue("@CategoryId", (object?)product.CategoryId ?? DBNull.Value);

                    command.ExecuteNonQuery();
                }
            }
        }

        // ==========================================
        // GET BY ID — UPDATED: Now includes category name
        // ==========================================
        public Product GetById(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // "Call the saved GetProductById procedure"
                using (SqlCommand command = new SqlCommand("GetProductById", connection))
                {
                    // "Tell C# this is a stored procedure"
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    // "Pass the ID to the procedure"
                    command.Parameters.AddWithValue("@Id", id);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Product product = new Product(
                                reader.GetString(1),
                                reader.GetDecimal(2),
                                reader.GetInt32(3)
                            );
                            product.Id = reader.GetInt32(0);
                            if (!reader.IsDBNull(4))
                                product.CategoryName = reader.GetString(4);
                            return product;
                        }
                        return null;
                    }
                }
            }
        }

        // ==========================================
        // UPDATE — UPDATED: Now includes CategoryId
        // ==========================================
        public void UpdateProduct(Product product)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // UPDATED: Added CategoryId to UPDATE
                string query = "UPDATE Products SET Name = @Name, Price = @Price, Stock = @Stock, CategoryId = @CategoryId WHERE Id = @Id";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", product.Id);
                    command.Parameters.AddWithValue("@Name", product.Name);
                    command.Parameters.AddWithValue("@Price", product.Price);
                    command.Parameters.AddWithValue("@Stock", product.Stock);
                    // NEW: Add CategoryId (can be null)
                    command.Parameters.AddWithValue("@CategoryId", (object?)product.CategoryId ?? DBNull.Value);
                    command.ExecuteNonQuery();
                }
            }
        }

        // ==========================================
        // DELETE — No changes needed
        // ==========================================
        public void DeleteProduct(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // "Call the saved DeleteProduct procedure"
                using (SqlCommand command = new SqlCommand("DeleteProduct", connection))
                {
                    // "Tell C# this is a stored procedure"
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    // "Pass the ID to the procedure"
                    command.Parameters.AddWithValue("@Id", id);

                    command.ExecuteNonQuery();
                }
            }
        }
        // "Get all categories from database"
        // "Get all categories from database"
        public List<Category> GetAllCategories()
        {
            var categories = new List<Category>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT Id, Name FROM Categories ORDER BY Name";

                using (SqlCommand command = new SqlCommand(query, connection))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        categories.Add(new Category
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1)
                        });
                    }
                }
            }
            return categories;
        }
    }
}
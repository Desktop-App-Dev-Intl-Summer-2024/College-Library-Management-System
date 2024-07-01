using MySql.Data.MySqlClient;
using Org.BouncyCastle.Tls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace FarmersMarket
{
    internal class Admin
    {
        private SqlConnection sql_conn;

        public Admin()
        {
            string connString = "Server=ACER\\MSSQLSERVER01;Database=FarmersMarket;Integrated Security=True;TrustServerCertificate=True;";
            sql_conn = new SqlConnection(connString);
            sql_conn.Open();
        }

        public async Task<ObservableCollection<Product>> GetProductsAsync()
        {
            return await Task.Run(() =>
            {
                var products = new ObservableCollection<Product>();
                string query = "SELECT * FROM Products";
                SqlCommand cmd = new SqlCommand(query, sql_conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    products.Add(new Product
                    {
                        ProductName = reader["ProductName"].ToString(),
                        ProductID = int.Parse(reader["ProductID"].ToString()),
                        Amount = decimal.Parse(reader["Amount"].ToString()),
                        Price = decimal.Parse(reader["Price"].ToString())
                    });
                }
                reader.Close();
                return products;
            });
        }

        public async Task AddProductAsync(Product product)
        {
            await Task.Run(() =>
            {
                string query = "INSERT INTO Products (ProductName, ProductID, Amount, Price) VALUES (@name, @id, @amount, @price)";
                SqlCommand cmd = new SqlCommand(query, sql_conn);
                cmd.Parameters.AddWithValue("@name", product.ProductName);
                cmd.Parameters.AddWithValue("@id", product.ProductID);
                cmd.Parameters.AddWithValue("@amount", product.Amount);
                cmd.Parameters.AddWithValue("@price", product.Price);
                cmd.ExecuteNonQuery();
            });
        }

        public async Task UpdateProductAsync(Product product)
        {
            await Task.Run(() =>
            {
                string query = "UPDATE Products SET Amount = @amount, Price = @price WHERE ProductID = @id";
                SqlCommand cmd = new SqlCommand(query, sql_conn);
                cmd.Parameters.AddWithValue("@amount", product.Amount);
                cmd.Parameters.AddWithValue("@price", product.Price);
                cmd.Parameters.AddWithValue("@id", product.ProductID);
                cmd.ExecuteNonQuery();
            });
        }

        public async Task DeleteProductAsync(int productId)
        {
            await Task.Run(() =>
            {
                string query = "DELETE FROM Products WHERE ProductID = @id";
                SqlCommand cmd = new SqlCommand(query, sql_conn);
                cmd.Parameters.AddWithValue("@id", productId);
                cmd.ExecuteNonQuery();
            });
        }
    }
}
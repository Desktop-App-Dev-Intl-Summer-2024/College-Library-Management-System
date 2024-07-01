using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmersMarket
{
    internal class Sales
    {
        private SqlConnection sql_conn;

        public Sales()
        {
            string connString = "Server=ACER\\MSSQLSERVER01;Database=FarmersMarket;Integrated Security=True;TrustServerCertificate=True;";
            sql_conn = new SqlConnection(connString);
            sql_conn.Open();
        }

        public async Task<decimal> CalculateTotalSalesAsync(Dictionary<int, decimal> purchases)
        {
            return await Task.Run(() =>
            {
                decimal total = 0;

                foreach (var purchase in purchases)
                {
                    string query = "SELECT Price, Amount FROM Products WHERE ProductID = @id";
                    SqlCommand cmd = new SqlCommand(query, sql_conn);
                    cmd.Parameters.AddWithValue("@id", purchase.Key);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        decimal price = decimal.Parse(reader["Price"].ToString());
                        decimal amount = decimal.Parse(reader["Amount"].ToString());
                        reader.Close();

                        if (amount >= purchase.Value)
                        {
                            total += purchase.Value * price;

                            // Update the amount in the database
                            decimal newAmount = amount - purchase.Value;
                            UpdateProductAmountAsync(purchase.Key, newAmount).Wait();
                        }
                        else
                        {
                            throw new Exception("Not enough stock for product ID: " + purchase.Key);
                        }
                    }
                }

                return total;
            });
        }

        private async Task UpdateProductAmountAsync(int productId, decimal newAmount)
        {
            await Task.Run(() =>
            {
                string query = "UPDATE Products SET Amount = @amount WHERE ProductID = @id";
                SqlCommand cmd = new SqlCommand(query, sql_conn);
                cmd.Parameters.AddWithValue("@amount", newAmount);
                cmd.Parameters.AddWithValue("@id", productId);
                cmd.ExecuteNonQuery();
            });
        }
    }
}
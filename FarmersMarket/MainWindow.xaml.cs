using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FarmersMarket
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Admin admin;
        private SqlConnection sql_conn;

        public MainWindow()
        {
            InitializeComponent();
            admin = new Admin();
            string connString = "Server=ACER\\MSSQLSERVER01;Database=FarmersMarket;Integrated Security=True;TrustServerCertificate=True;";
            sql_conn = new SqlConnection(connString);
            sql_conn.Open();
            InitializeTextBoxPlaceholders();
        }

        private void InitializeTextBoxPlaceholders()
        {
            productNameTextBox.Tag = "Product Name";
            productIDTextBox.Tag = "Product ID";
            amountTextBox.Tag = "Amount";
            priceTextBox.Tag = "Price";

            SetPlaceholder(productNameTextBox);
            SetPlaceholder(productIDTextBox);
            SetPlaceholder(amountTextBox);
            SetPlaceholder(priceTextBox);
        }

        private void SetPlaceholder(TextBox textBox)
        {
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = textBox.Tag.ToString();
                textBox.Foreground = System.Windows.Media.Brushes.Gray;
            }
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox != null && textBox.Text == textBox.Tag.ToString())
            {
                textBox.Text = string.Empty;
                textBox.Foreground = System.Windows.Media.Brushes.Black;
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox != null && string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = textBox.Tag.ToString();
                textBox.Foreground = System.Windows.Media.Brushes.Gray;
            }
        }

        private async void LoadProductsButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var products = await admin.GetProductsAsync();
                productListView.ItemsSource = products;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading products: " + ex.Message);
            }
        }

        private async void AddProductButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var product = new Product
                {
                    ProductName = productNameTextBox.Text,
                    ProductID = int.Parse(productIDTextBox.Text),
                    Amount = decimal.Parse(amountTextBox.Text),
                    Price = decimal.Parse(priceTextBox.Text)
                };

                await admin.AddProductAsync(product);
                LoadProductsButton_Click(sender, e); // Reload products to reflect changes
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding product: " + ex.Message);
            }
        }

        private async void UpdateProductButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedProduct = productListView.SelectedItem as Product;
                if (selectedProduct != null)
                {
                    selectedProduct.ProductName = productNameTextBox.Text;
                    selectedProduct.Amount = decimal.Parse(amountTextBox.Text);
                    selectedProduct.Price = decimal.Parse(priceTextBox.Text);

                    await admin.UpdateProductAsync(selectedProduct);
                    LoadProductsButton_Click(sender, e); // Reload products to reflect changes
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating product: " + ex.Message);
            }
        }

        private async void DeleteProductButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedProduct = productListView.SelectedItem as Product;
                if (selectedProduct != null)
                {
                    await admin.DeleteProductAsync(selectedProduct.ProductID);
                    LoadProductsButton_Click(sender, e); // Reload products to reflect changes
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting product: " + ex.Message);
            }
        }

        private async void CalculateSalesButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int productId = int.Parse(salesProductIDTextBox.Text);
                decimal amount = decimal.Parse(salesAmountTextBox.Text);

                decimal totalSales = await CalculateTotalSalesAsync(productId, amount);
                totalSalesTextBlock.Text = $"Total Sales: {totalSales:C}";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error calculating sales: " + ex.Message);
            }
        }

        private async Task<decimal> CalculateTotalSalesAsync(int productId, decimal purchaseAmount)
        {
            return await Task.Run(() =>
            {
                decimal total = 0;

                string query = "SELECT Price, Amount FROM Products WHERE ProductID = @id";
                SqlCommand cmd = new SqlCommand(query, sql_conn);
                cmd.Parameters.AddWithValue("@id", productId);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    decimal price = decimal.Parse(reader["Price"].ToString());
                    decimal amount = decimal.Parse(reader["Amount"].ToString());
                    reader.Close();

                    if (amount >= purchaseAmount)
                    {
                        total = purchaseAmount * price;

                        // Update the amount in the database
                        decimal newAmount = amount - purchaseAmount;
                        UpdateProductAmountAsync(productId, newAmount).Wait();
                    }
                    else
                    {
                        throw new Exception("Not enough stock for product ID: " + productId);
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
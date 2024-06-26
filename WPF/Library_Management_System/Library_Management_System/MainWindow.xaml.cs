using System;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

namespace Library_Management_System
{
    public partial class MainWindow : Window
    {
        private readonly string connectionString;
        private SqlConnection sqlConnection;

        public MainWindow()
        {
            InitializeComponent();
            connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["LibraryConnectionString"].ConnectionString;
        }

        private void Establish_Connection()
        {
            sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Establish_Connection();

                string username = LoginUsernameTextBox.Text;
                string password = LoginPasswordBox.Password;

                if (AuthenticateUser(username, password))
                {
                    MessageBox.Show("Login successful!");
                }
                else
                {
                    MessageBox.Show("Invalid username or password.");
                }

                sqlConnection.Close();
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show($"SQL Error: {sqlEx.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void SignupButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Establish_Connection();

                string username = SignupUsernameTextBox.Text;
                string password = SignupPasswordBox.Password;

                if (RegisterUser(username, password))
                {
                    MessageBox.Show("Sign up successful! You can now log in.");
                }
                else
                {
                    MessageBox.Show("Sign up failed. Username may already be taken.");
                }

                sqlConnection.Close();
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show($"SQL Error: {sqlEx.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private bool AuthenticateUser(string username, string password)
        {
            string query = "SELECT PasswordHash FROM Users WHERE Username = @Username";
            SqlCommand command = new SqlCommand(query, sqlConnection);
            command.Parameters.AddWithValue("@Username", username);

            string storedHash = (string)command.ExecuteScalar();

            if (storedHash != null && VerifyPasswordHash(password, storedHash))
            {
                return true;
            }
            return false;
        }

        private bool RegisterUser(string username, string password)
        {
            string query = "INSERT INTO Users (Username, PasswordHash) VALUES (@Username, @PasswordHash)";
            SqlCommand command = new SqlCommand(query, sqlConnection);
            command.Parameters.AddWithValue("@Username", username);
            command.Parameters.AddWithValue("@PasswordHash", HashPassword(password));

            try
            {
                command.ExecuteNonQuery();
                return true;
            }
            catch (SqlException)
            {
                return false;
            }
        }

        private static string HashPassword(string password)
        {
            byte[] bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }
            return builder.ToString();
        }

        private bool VerifyPasswordHash(string password, string storedHash)
        {
            string hashOfInput = HashPassword(password);
            return hashOfInput.Equals(storedHash, StringComparison.OrdinalIgnoreCase);
        }
    }
}

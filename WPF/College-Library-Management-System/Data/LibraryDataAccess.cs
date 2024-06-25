using College_Library_Management_System.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollegeLibraryManagementSystem.Data
{
    internal class LibraryDataAccess
{
        private readonly string _connectionString;

        public LibraryDataAccess()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["LibraryConnectionString"].ConnectionString;
        }

        public List<Book> GetAllBooks()
        {
            List<Book> books = new List<Book>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Books";
                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Book book = new Book
                    {
                        BookID = Convert.ToInt32(reader["BookID"]),
                        Title = reader["Title"].ToString(),
                        Author = reader["Author"].ToString(),
                        Genre = reader["Genre"].ToString(),
                        ISBN = reader["ISBN"].ToString(),
                        Publisher = reader["Publisher"].ToString(),
                        YearPublished = Convert.ToInt32(reader["YearPublished"]),
                        CopiesAvailable = Convert.ToInt32(reader["CopiesAvailable"])
                    };
                    books.Add(book);
                }

                reader.Close();
            }

            return books;
        }

        public List<Member> GetAllMembers()
        {
            List<Member> members = new List<Member>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Members";
                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Member member = new Member
                    {
                        MemberID = Convert.ToInt32(reader["MemberID"]),
                        FirstName = reader["FirstName"].ToString(),
                        LastName = reader["LastName"].ToString(),
                        Email = reader["Email"].ToString(),
                        Phone = reader["Phone"].ToString(),
                        Address = reader["Address"].ToString(),
                        RegistrationDate = Convert.ToDateTime(reader["RegistrationDate"]),
                        Role = reader["Role"].ToString()
                    };
                    members.Add(member);
                }

                reader.Close();
            }

            return members;
        }

        internal void AddBook(Book book)
        {
            throw new NotImplementedException();
        }

        internal void AddMember(Member member)
        {
            throw new NotImplementedException();
        }
    }
}
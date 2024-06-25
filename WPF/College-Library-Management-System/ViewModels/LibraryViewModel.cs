using College_Library_Management_System.Models;
using CollegeLibraryManagementSystem.Data;
using System.Collections.ObjectModel;

namespace College_Library_Management_System.ViewModels
{
    internal class LibraryViewModel
    {
        private LibraryDataAccess _dataAccess;

        public ObservableCollection<Book> Books { get; set; }
        public ObservableCollection<Member> Members { get; set; }

        public LibraryViewModel()
        {
            _dataAccess = new LibraryDataAccess();
            Books = new ObservableCollection<Book>(_dataAccess.GetAllBooks());
            Members = new ObservableCollection<Member>(_dataAccess.GetAllMembers());
        }

        public void AddBook(Book book)
        {
            _dataAccess.AddBook(book);
            Books.Add(book);
        }

        public void AddMember(Member member)
        {
            _dataAccess.AddMember(member);
            Members.Add(member);
        }
    }
}

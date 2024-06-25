using College_Library_Management_System.Models;
using CollegeLibraryManagementSystem.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace College_Library_Management_System.ViewModels
{
    internal class MainViewModel : INotifyPropertyChanged
    {
        private LibraryDataAccess _dataAccess;

        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<Book> _books;
        public ObservableCollection<Book> Books
        {
            get { return _books; }
            set
            {
                _books = value;
                OnPropertyChanged(nameof(Books));
            }
        }

        private ObservableCollection<Member> _members;
        public ObservableCollection<Member> Members
        {
            get { return _members; }
            set
            {
                _members = value;
                OnPropertyChanged(nameof(Members));
            }
        }

        public MainViewModel()
        {
            _dataAccess = new LibraryDataAccess();
            LoadData();
        }

        private void LoadData()
        {
            Books = new ObservableCollection<Book>(_dataAccess.GetAllBooks());
            Members = new ObservableCollection<Member>(_dataAccess.GetAllMembers());
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace College_Library_Management_System.Models
{
    internal class Book
    {
        public int BookID { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Genre { get; set; }
        public string ISBN { get; set; }
        public string Publisher { get; set; }
        public int YearPublished { get; set; }
        public int CopiesAvailable { get; set; }
    }
}


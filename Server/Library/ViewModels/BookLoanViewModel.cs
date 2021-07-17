using System;

namespace Library.ViewModels
{
    public class BookLoanViewModel
    {
        public string BookId { get; set; }

        public string BookTitle { get; set; }

        public string BookAuthor { get; set; }

        public string User { get; set; }

        public DateTime Borrowed { get; set; }

        public DateTime Returned { get; set; }
    }
}
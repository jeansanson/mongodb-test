using Library.Models;

namespace Library.ViewModels
{
    public class BookFilterViewModel
    {
        public string Title { get; set; }

        public string Author { get; set; }

        public BookLoanFilter ToModel()
        {
            return new BookLoanFilter
            {
                Author = Author,
                Title = Title,
                BeenLoaned = false
            };
        }
    }
}
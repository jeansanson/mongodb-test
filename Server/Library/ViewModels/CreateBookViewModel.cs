using Library.Models;
using System.ComponentModel.DataAnnotations;

namespace Library.ViewModels
{
    public class CreateBookViewModel
    {
        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        [Required]
        [MaxLength(200)]
        public string Author { get; set; }

        public BookLoan ToModel()
        {
            return new BookLoan
            {
                Author = Author,
                Title = Title
            };
        }
    }
}
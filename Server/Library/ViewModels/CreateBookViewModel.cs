using Library.Models;
using System.Collections.Generic;
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

        public Book ToModel()
        {
            return new Book
            {
                Author = Author,
                Title = Title,
                Loans = new List<Loan>()
            };
        }
    }
}
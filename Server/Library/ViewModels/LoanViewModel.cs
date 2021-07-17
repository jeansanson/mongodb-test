using System;
using System.ComponentModel.DataAnnotations;

namespace Library.ViewModels
{
    public class LoanViewModel
    {
        [Required]
        public string BookId { get; set; }

        [Required]
        public string User { get; set; }

        [Required]
        public DateTime Borrowed { get; set; }

        public DateTime? Returned { get; set; }
    }
}
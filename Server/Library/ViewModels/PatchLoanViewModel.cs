using System;
using System.ComponentModel.DataAnnotations;

namespace Library.ViewModels
{
    public class PatchLoanViewModel
    {
        [Required]
        public string BookId { get; set; }

        [Required]
        public string Id { get; set; }

        [Required]
        public string User { get; set; }

        public DateTime Returned { get; set; }
    }
}
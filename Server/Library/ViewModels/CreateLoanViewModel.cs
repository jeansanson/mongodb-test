using System;
using System.ComponentModel.DataAnnotations;

namespace Library.ViewModels
{
    public class CreateLoanViewModel
    {
        [Required]
        public string User { get; set; }
    }
}
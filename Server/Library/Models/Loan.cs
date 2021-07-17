using System;

namespace Library.Models
{
    public class Loan
    {
        public string Guid { get; set; }

        public string User { get; set; }

        public DateTime Borrowed { get; set; }

        public DateTime Returned { get; set; }
    }
}
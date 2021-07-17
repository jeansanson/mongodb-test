namespace Library.Models
{
    public class BookLoanFilter
    {
        public string? Id { get; set; }

        public string? Title { get; set; }

        public string? Author { get; set; }

        public bool BeenLoaned { get; set; }
    }
}
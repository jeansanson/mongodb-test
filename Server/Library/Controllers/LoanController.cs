using Library.Models;
using Library.Services;
using Library.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Library.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoanController : ControllerBase
    {
        private readonly BookService _bookService;

        public LoanController(BookService bookService)
        {
            _bookService = bookService;
        }

        /// <summary>
        /// Obter todos os empréstimos.
        /// </summary>
        [HttpGet]
        public ActionResult<List<BookLoanViewModel>> Get() =>
            ToViewModel(_bookService.Get(LoanedOnly()));

        /// <summary>
        /// Obter um empréstimo para um livro.
        /// </summary>
        [Route("{id:length(24)}")]
        [HttpGet]
        public ActionResult<BookLoanViewModel> Get([FromRoute] string id)
        {
            BookLoan bookLoan = _bookService.Get(id);
            if (bookLoan == null)
            {
                return NotFound();
            }
            if (!bookLoan.Loaned)
            {
                return BadRequest("Book isn't loaned.");
            }
            return ToViewModel(bookLoan);
        }

        /// <summary>
        /// Atualiza um empréstimo.
        /// </summary>
        [Route("{id:length(24)}")]
        [HttpPatch]
        public IActionResult Update([FromRoute] string id, [FromBody] LoanViewModel loanViewModel)
        {
            if (id != loanViewModel.BookId)
            {
                return BadRequest("Body and url parameters must match.");
            }

            BookLoan bookLoan = _bookService.Get(id);
            if (bookLoan == null) return NotFound();
            bookLoan.Borrowed = loanViewModel.Borrowed;
            if (loanViewModel.Returned.HasValue)
            {
                if (bookLoan.Borrowed < loanViewModel.Returned.Value)
                {
                    return BadRequest("Returned date must be greather than borrowed date.");
                }
                bookLoan.Returned = loanViewModel.Returned.Value;
            }
            bookLoan.User = loanViewModel.User;
            _bookService.Update(id, bookLoan);
            return NoContent();
        }

        private static BookLoanViewModel ToViewModel(BookLoan bookLoan)
        {
            return new BookLoanViewModel
            {
                BookId = bookLoan.Id,
                BookTitle = bookLoan.Title,
                BookAuthor = bookLoan.Author,
                User = bookLoan.User,
                Borrowed = bookLoan.Borrowed,
                Returned = bookLoan.Returned
            };
        }

        private static List<BookLoanViewModel> ToViewModel(List<BookLoan> bookLoan)
        {
            var result = new List<BookLoanViewModel>();
            bookLoan.ForEach(model => { result.Add(ToViewModel(model)); });
            return result;
        }

        private static BookLoanFilter LoanedOnly(string id = null)
        {
            return new BookLoanFilter
            {
                Id = id,
                BeenLoaned = true
            };
        }
    }
}
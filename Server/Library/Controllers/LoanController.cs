using Library.Models;
using Library.Services;
using Library.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
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
        /// Obter empréstimos de um livro.
        /// </summary>
        [Route("{bookId:length(24)}")]
        [HttpGet]
        public ActionResult<LoanViewModel> Get([FromRoute] string bookId)
        {
            Book book = _bookService.Get(bookId);
            if (book == null) return NotFound("Book not found");
            return Ok(book.Loans);
        }

        /// <summary>
        /// Atualizar um empréstimo.
        /// </summary>
        [Route("{bookId:length(24)}")]
        [HttpPatch]
        public IActionResult Update([FromRoute] string bookId, [FromBody] PatchLoanViewModel loanViewModel)
        {
            if (bookId != loanViewModel.BookId)
            {
                return BadRequest("Body and url parameters must match.");
            }

            Book bookLoan = _bookService.Get(bookId);
            if (bookLoan == null) return NotFound("Book not found");
            Loan loan = bookLoan.GetLoan(loanViewModel.Id);
            if (loan == null) return NotFound();
            loan.User = loanViewModel.User;
            loan.Returned = loanViewModel.Returned;
            if (!IsLoanDateValid(loan))
            {
                return BadRequest("Returned date must be greather than borrowed date.");
            }
            bookLoan.UpdateLoan(loan);
            _bookService.Update(bookId, bookLoan);
            return NoContent();
        }

        /// <summary>
        /// Inserir um empréstimo.
        /// </summary>
        [Route("{bookId:length(24)}")]
        [HttpPost]
        public IActionResult Create([FromRoute] string bookId, [FromBody] CreateLoanViewModel loanViewModel)
        {
            Book book = _bookService.Get(bookId);
            if (book == null) return NotFound("Book not found");
            if (!book.IsAvaiable()) return BadRequest("Book unavailable");
            Loan loan = book.AddLoan(loanViewModel.User);
            if (!IsLoanDateValid(loan))
            {
                return BadRequest("Returned date must be greather than borrowed date.");
            }
            _bookService.Update(bookId, book);
            return Ok(loan);
        }

        private static bool IsLoanDateValid(Loan loan)
        {
            if (loan.Returned > DateTime.MinValue)
            {
                return loan.Borrowed < loan.Returned;
            }
            return true;
        }
    }
}
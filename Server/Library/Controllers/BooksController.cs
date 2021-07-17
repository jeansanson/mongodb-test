using Library.Models;
using Library.Services;
using Library.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Library.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly BookService _bookService;

        public BooksController(BookService bookService)
        {
            _bookService = bookService;
        }

        /// <summary>
        /// Obter todos os livros.
        /// </summary>
        [HttpGet]
        public ActionResult<List<BookViewModel>> Get([FromQuery] BookFilterViewModel query = null)
        {
            if (query != null)
            {
                return ToViewModel(_bookService.Get(query.ToModel()));
            }
            return ToViewModel(_bookService.Get());
        }

        /// <summary>
        /// Obter livro por id.
        /// </summary>
        [HttpGet("{id:length(24)}", Name = "GetBook")]
        public ActionResult<BookViewModel> Get(string id)
        {
            BookLoan bookLoan = _bookService.Get(id);
            return bookLoan == null ? NotFound() : ToViewModel(bookLoan);
        }

        /// <summary>
        /// Inserir livro.
        /// </summary>
        [HttpPost]
        public ActionResult<BookViewModel> Create(CreateBookViewModel createBookViewModel)
        {
            BookLoan bookLoan = createBookViewModel.ToModel();
            _bookService.Create(bookLoan);
            return CreatedAtRoute("GetBook", new { id = bookLoan.Id.ToString() }, bookLoan);
        }

        /// <summary>
        /// Atualizar livro.
        /// </summary>
        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, BookViewModel bookViewModel)
        {
            if (id != bookViewModel.Id)
            {
                return BadRequest("Body and url parameters must match.");
            }

            BookLoan bookLoan = _bookService.Get(id);
            if (bookLoan == null)
            {
                return NotFound();
            }
            bookLoan.Author = bookViewModel.Author;
            bookLoan.Title = bookViewModel.Title;
            _bookService.Update(id, bookLoan);
            return NoContent();
        }

        /// <summary>
        /// Deletar livro.
        /// </summary>
        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            BookLoan book = _bookService.Get(id);

            if (book == null)
            {
                return NotFound();
            }

            _bookService.Remove(book.Id);
            return NoContent();
        }

        private static BookViewModel ToViewModel(BookLoan bookLoan)
        {
            return new BookViewModel
            {
                Id = bookLoan.Id,
                Author = bookLoan.Author,
                Title = bookLoan.Title
            };
        }

        private static List<BookViewModel> ToViewModel(List<BookLoan> bookLoan)
        {
            var result = new List<BookViewModel>();
            bookLoan.ForEach(model => { result.Add(ToViewModel(model)); });
            return result;
        }
    }
}
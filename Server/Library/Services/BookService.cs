using Library.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Library.Services
{
    public class BookService
    {
        private readonly IMongoCollection<BookLoan> _books;

        public BookService(IDatabaseSettings settings)
        {
            // inicialização feia, rever depois (injeção do client)
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _books = database.GetCollection<BookLoan>(settings.BooksCollectionName);
        }

        public List<BookLoan> Get() =>
            _books.Find(book => true).ToList();

        public BookLoan Get(string id) =>
            _books.Find(book => book.Id == id).FirstOrDefault();

        public List<BookLoan> Get(BookLoanFilter filter)
        {
            if (filter == null) throw new ArgumentNullException(nameof(filter));

            // bad feelings StringComparison unsupported
            var builder = Builders<BookLoan>.Filter;
            var builderFilter = builder.Empty;

            if (filter.Id != null && filter.Id.Length > 0)
            {
                builderFilter &= builder.Where(book => book.Id.Contains(filter.Id));
            }

            if (filter.Author != null && filter.Author.Length > 0)
            {
                builderFilter &= builder.Where(book => book.Author.Contains(filter.Author));
            }

            if (filter.Title != null && filter.Title.Length > 0)
            {
                builderFilter &= builder.Where(book => book.Title.Contains(filter.Title));
            }

            if (filter.BeenLoaned)
            {
                // book.Loaned not supported
                builderFilter &= builder.Where(book => book.Borrowed > DateTime.MinValue || book.Returned > DateTime.MinValue);
            }

            return _books.Find(builderFilter).ToList();
        }

        public BookLoan Create(BookLoan book)
        {
            _books.InsertOne(book);
            return book;
        }

        public void Update(string id, BookLoan bookIn) =>
            _books.ReplaceOne(book => book.Id == id, bookIn);

        public void Remove(BookLoan bookIn) =>
            _books.DeleteOne(book => book.Id == bookIn.Id);

        public void Remove(string id) =>
            _books.DeleteOne(book => book.Id == id);
    }
}
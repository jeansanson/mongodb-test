using Library.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Library.Services
{
    public class BookService
    {
        private readonly IMongoCollection<Book> _books;

        public BookService(IDatabaseSettings settings)
        {
            // inicialização feia, rever depois (injeção do client)
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _books = database.GetCollection<Book>(settings.BooksCollectionName);
        }

        public List<Book> Get() =>
            _books.Find(book => true).ToList();

        public Book Get(string id) =>
            _books.Find(book => book.Id == id).FirstOrDefault();

        public List<Book> Get(BookLoanFilter filter)
        {
            if (filter == null) throw new ArgumentNullException(nameof(filter));

            var builder = Builders<Book>.Filter;
            var builderFilter = builder.Empty;

            if (filter.Id != null && filter.Id.Length > 0)
            {
                // bad feelings StringComparison unsupported
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
                builderFilter &= builder.Where(book => book.Loans.Count > 0);
            }

            return _books.Find(builderFilter).ToList();
        }

        public Book Create(Book book)
        {
            _books.InsertOne(book);
            return book;
        }

        public void Update(string id, Book bookIn) =>
            _books.ReplaceOne(book => book.Id == id, bookIn);

        public void Remove(Book bookIn) =>
            _books.DeleteOne(book => book.Id == bookIn.Id);

        public void Remove(string id) =>
            _books.DeleteOne(book => book.Id == id);
    }
}
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Library.Models
{
    public class BookLoan
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Title { get; set; }

        public string Author { get; set; }

        public string User { get; set; }

        public DateTime Borrowed { get; set; }

        public DateTime Returned { get; set; }

        public bool Loaned { get { return Borrowed > DateTime.MinValue & Returned == DateTime.MinValue; } }
    }
}
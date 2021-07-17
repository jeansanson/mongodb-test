using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Library.Models
{
    public class Book
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Title { get; set; }

        public string Author { get; set; }

        public List<Loan> Loans { get; set; }

        public bool IsAvaiable()
        {
            if (Loans == null || Loans.Count == 0) return true;
            Loans.Sort((x, y) => DateTime.Compare(x.Returned, y.Returned));
            return Loans[0].Returned > DateTime.MinValue;
        }

        public Loan AddLoan(string user)
        {
            if (Loans == null) Loans = new List<Loan>();
            Loan loan = new Loan()
            {
                Guid = Guid.NewGuid().ToString(),
                User = user,
                Borrowed = DateTime.Now
            };
            Loans.Add(loan);
            return loan;
        }

        public void UpdateLoan(Loan loan)
        {
            if (Loans == null) Loans = new List<Loan>();
            Loan existingLoan = GetLoan(loan.Guid);
            if (existingLoan != null)
            {
                existingLoan.User = loan.User;
                existingLoan.Borrowed = loan.Borrowed;
                existingLoan.Returned = loan.Returned;
            }
        }

        public bool LoanExists(string id)
        {
            return Loans.Exists(loan => loan.Guid == id);
        }

        public Loan GetLoan(string id)
        {
            return Loans.FindLast(loan => loan.Guid == id);
        }
    }
}
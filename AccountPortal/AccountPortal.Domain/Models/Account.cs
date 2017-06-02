using System;
using System.Collections.Generic;
using System.Text;

namespace AccountPortal.Domain.Models
{
    public class Account
    {
        public Account()
        {
            Transactions = new List<Transaction>();
            Messages = new List<string>();
        }
        public string Username { get; set; }
        public string Password { get; set; }
        public decimal AccountBalance { get; set; }
        public List<Transaction> Transactions { get; set; }
        public List<string> Messages { get; set; }
    }
}

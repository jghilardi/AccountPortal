using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountPortal.Domain.Models
{
    public class Transaction
    {
        public decimal Amount { get; set; }
        public bool IsDeposit { get; set; }
        public DateTime SubmittedDate { get; set; }
    }
}

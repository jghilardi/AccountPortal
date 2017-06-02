using System;
using AccountPortal.Domain.Models;
using AccountPortal.Domain.Processors.Interfaces;

namespace AccountPortal.Domain.Processors
{
    public class TransactionProcessor : ITransactionProcessor
    {
        public Account Deposit(Account account, string amount)
        {
            var depositAmount = 0m;
            decimal.TryParse(amount, out depositAmount);
            if (depositAmount > 0 && depositAmount <= 99999)
            {
                account.AccountBalance += depositAmount;
                account.Transactions.Add(new Transaction
                {
                    Amount = depositAmount,
                    IsDeposit = true,
                    SubmittedDate = DateTime.Now
                }); 
            }
            else
            {
                account.Messages.Add("Invalid amount.  Please try again.");
            }
            return account;
        }

        public Account Withdraw(Account account, string amount)
        {
            var withdrawAmount = 0m;
            decimal.TryParse(amount, out withdrawAmount);
            if (withdrawAmount <= 0 || account.AccountBalance >= withdrawAmount)
            {
                account.AccountBalance -= withdrawAmount;
                account.Transactions.Add(new Transaction
                {
                    Amount = withdrawAmount,
                    IsDeposit = true,
                    SubmittedDate = DateTime.Now
                });
            }
            else
            {
                account.Messages.Add("Insufficient funds or invalid input.");
            }
            return account;
        }
    }
}

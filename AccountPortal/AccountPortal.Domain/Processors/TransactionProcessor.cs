using System;
using AccountPortal.Domain.Models;
using AccountPortal.Domain.Processors.Interfaces;
using System.Globalization;

namespace AccountPortal.Domain.Processors
{
    public class TransactionProcessor : ITransactionProcessor
    {
        public Account Deposit(Account account, string amount)
        {
            try
            {
                var depositAmount = 0m;
                decimal.TryParse(amount, out depositAmount);
                if (depositAmount > 0 && depositAmount <= 99999 && LessThanThreeDecimalPlaces(depositAmount))
                {
                    account.AccountBalance += depositAmount;
                    account.Transactions.Add(new Transaction
                    {
                        IsDeposit = true,
                        Amount = depositAmount,
                        SubmittedDate = DateTime.Now
                    });
                }
                else
                {
                    account.Messages.Add("Invalid amount.  Please try again.");
                }
            }
            catch (Exception)
            {
                //log exception
                account.Messages.Add("Transaction error");
            }
            return account;
        }

        public Account Withdraw(Account account, string amount)
        {
            try
            {
                var withdrawAmount = 0m;
                decimal.TryParse(amount, out withdrawAmount);
                if (withdrawAmount <= 0 || account.AccountBalance >= withdrawAmount)
                {
                    account.AccountBalance -= withdrawAmount;
                    account.Transactions.Add(new Transaction
                    {
                        Amount = withdrawAmount,
                        SubmittedDate = DateTime.Now
                    });
                }
                else
                {
                    account.Messages.Add("Insufficient funds or invalid input.");
                }
            }
            catch (Exception)
            {
                //log exception
                account.Messages.Add("Transaction error");
            }
            return account;
        }

        public bool LessThanThreeDecimalPlaces(decimal input)
        {
            var response = false;
            if (Decimal.Round(input, 2) == input)
            {
                response = true;
            }
            return response;
        }
    }
}

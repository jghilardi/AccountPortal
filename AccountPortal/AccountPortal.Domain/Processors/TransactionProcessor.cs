using System;
using AccountPortal.Domain.Models;
using AccountPortal.Domain.Processors.Interfaces;

namespace AccountPortal.Domain.Processors
{
    public class TransactionProcessor : ITransactionProcessor
    {
        public Account Deposit(Account account, string amount)
        {
            try
            {
                decimal.TryParse(amount, out var depositAmount);
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
            catch (Exception ex)
            {
                account.Messages.Add($"Transaction error: {ex}");
            }
            return account;
        }

        public Account Withdraw(Account account, string amount)
        {
            try
            {
                decimal.TryParse(amount, out var withdrawalAmount);
                if (withdrawalAmount <= 0 || account.AccountBalance >= withdrawalAmount)
                {
                    account.AccountBalance -= withdrawalAmount;
                    account.Transactions.Add(new Transaction
                    {
                        Amount = withdrawalAmount,
                        SubmittedDate = DateTime.Now
                    });
                }
                else
                {
                    account.Messages.Add("Insufficient funds or invalid input.");
                }
            }
            catch (Exception ex)
            {
                account.Messages.Add($"Transaction error: {ex}");
            }
            return account;
        }

        public bool LessThanThreeDecimalPlaces(decimal input)
        {
            return decimal.Round(input, 2) == input;
        }
    }
}

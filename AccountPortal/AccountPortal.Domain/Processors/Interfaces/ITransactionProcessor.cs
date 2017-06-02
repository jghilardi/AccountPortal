using System.Collections.Generic;
using AccountPortal.Domain.Models;
using LazyCache;

namespace AccountPortal.Domain.Processors.Interfaces
{
    public interface ITransactionProcessor
    {
        Account Deposit(Account account, string amount);
        Account Withdraw(Account account, string amount);
        bool LessThanThreeDecimalPlaces(decimal input);
    }
}

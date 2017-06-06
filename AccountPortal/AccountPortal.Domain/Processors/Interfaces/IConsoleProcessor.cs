using AccountPortal.Domain.Models;
using LazyCache;

namespace AccountPortal.Domain.Processors.Interfaces
{
    public interface IConsoleProcessor
    {
        void Execute();
        void GetRootMenu(IAppCache cache);
        void GetTransactionMenu(IAppCache cache, Account activeUser);
        void ShowHistoricalTransactions(Account activeUser);
        int DisplayRootMenu();
        int ValidateMenuInput();
        Account LoginAccount(IAppCache cache);
        Account AddNewAccount(IAppCache cache);
        int DisplayTransactionMenu(decimal accountBalance);
        void UpdateCache(IAppCache cache, Account activeUser);
    }
}

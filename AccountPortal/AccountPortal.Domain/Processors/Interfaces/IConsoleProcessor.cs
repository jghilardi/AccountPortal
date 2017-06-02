using AccountPortal.Domain.Models;
using LazyCache;

namespace AccountPortal.Domain.Processors.Interfaces
{
    public interface IConsoleProcessor
    {
        void Execute();
        int DisplayRootMenu();
        Account LoginAccount(IAppCache cache);
        Account AddNewAccount(IAppCache cache);
    }
}

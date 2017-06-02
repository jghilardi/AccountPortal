using AccountPortal.Domain.Models;
using LazyCache;

namespace AccountPortal.Domain.Processors.Interfaces
{
    public interface IAccountProcessor
    {
        Account AddAccount(IAppCache cache, Account account);
        Account GetAccount(IAppCache cache, Account account);
    }
}

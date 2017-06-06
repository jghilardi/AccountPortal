using AccountPortal.Domain.Models;
using LazyCache;

namespace AccountPortal.Domain.Processors.Interfaces
{
    public interface IAccountProcessor
    {
        Account AddAccount(Account account);
        Account GetAccount(Account account);
    }
}

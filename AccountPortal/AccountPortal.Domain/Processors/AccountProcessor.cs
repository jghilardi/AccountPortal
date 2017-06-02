using System;
using System.Linq;
using AccountPortal.Domain.Extensions;
using AccountPortal.Domain.Models;
using AccountPortal.Domain.Processors.Interfaces;
using LazyCache;

namespace AccountPortal.Domain.Processors
{
    public class AccountProcessor : IAccountProcessor
    {
        private readonly IEncryptionUtility _encryptionUtility;

        public AccountProcessor(IEncryptionUtility encryptionUtility)
        {
            _encryptionUtility = encryptionUtility;
        }
        public Account AddAccount(IAppCache cache, Account account)
        {
            var response = new Account();
            try
            {
                var getAccount = cache.Get<Account>(account.Username);
                if (getAccount == null && account.Username.Length < 50 && account.Password.Length > 3) // regex??
                {
                    account.Password = _encryptionUtility.Encrypt(account.Password);
                    cache.Add(account.Username, account);
                    response.Username = account.Username;
                    response.AccountBalance = account.AccountBalance;
                }
                else
                {
                    response.Messages.Add("Invalid username or password.  Please try again.");
                }
            }
            catch (Exception ex)
            {
                response.Messages.Add("An unknown error occured.");
                //log exception
            }
            return response;
        }


        public Account GetAccount(IAppCache cache, Account account)
        {
            var response = new Account();
            try
            {
                var getAccount = cache.Get<Account>(account.Username);
                if (getAccount != null && account.Password == _encryptionUtility.Decrypt(getAccount.Password))
                {
                    response.Username = getAccount.Username;
                    response.AccountBalance = getAccount.AccountBalance;
                }
                else
                {
                    response.Messages.Add("Invalid username or password.");
                }

            }
            catch (Exception ex)
            {
                response.Messages.Add("An unknown error occured.");
                //log exception
            }
            return response;
        }
    }
}

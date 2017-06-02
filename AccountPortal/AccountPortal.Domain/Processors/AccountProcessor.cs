using System;
using System.Linq;
using AccountPortal.Domain.Extensions;
using AccountPortal.Domain.Models;
using AccountPortal.Domain.Processors.Interfaces;
using LazyCache;
using System.Collections.Generic;

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
                if (getAccount == null && account.Username.Length < 50 && account.Password.Length > 3)
                {
                    account.Password = _encryptionUtility.Encrypt(account.Password);
                    cache.Add(account.Username, account);
                    var accountResponse = new Account
                    {
                        Username = account.Username,
                        AccountBalance = account.AccountBalance,
                        Password = account.Password,
                        Transactions = account.Transactions
                    };
                    response = accountResponse;
                }
                else
                {
                    response.Messages.Add("Invalid username or password.");
                }
            }
            catch (Exception)
            {
                //log exception
                response.Messages.Add("An unknown error occured.");
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
                    var accountResponse = new Account
                    {
                        Username = account.Username,
                        AccountBalance = account.AccountBalance,
                        Password = account.Password,
                        Transactions = account.Transactions
                    };
                    response = accountResponse;
                }
                else
                {
                    response.Messages.Add("Invalid username or password.");
                }

            }
            catch (Exception)
            {
                //log exception
                response.Messages.Add("An unknown error occured.");
            }
            return response;
        }

        public void UpdateCache(IAppCache cache, Account activeUser)
        {
            cache.Remove(activeUser.Username);
            cache.Add(activeUser.Username, activeUser);
        }
    }
}

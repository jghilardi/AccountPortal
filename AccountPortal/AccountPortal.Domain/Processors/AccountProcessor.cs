using System;
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

        public Account AddAccount(Account account)
        {
            var response = new Account();
            try
            {
                if (account.Username.Length < 50 && account.Password.Length > 3)
                {
                    account.Password = _encryptionUtility.Encrypt(account.Password);
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
                    response.Messages.Add("Invalid user name or password.");
                }
            }
            catch (Exception)
            {
                response.Messages.Add("An unknown error occurred.");
            }
            return response;
        }

        public Account GetAccount(Account account, string password)
        {
            var response = new Account();
            try
            {
                if (account != null && password == _encryptionUtility.Decrypt(account.Password))
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
                    response.Messages.Add("Invalid user name or password.");
                }
            }
            catch (Exception ex)
            {
                //log exception
                response.Messages.Add("An unknown error occurred. Exception: " + ex);
            }
            return response;
        }
    }
}

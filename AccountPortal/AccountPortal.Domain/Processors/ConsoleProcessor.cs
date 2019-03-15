using System;
using AccountPortal.Data;
using AccountPortal.Domain.Models;
using AccountPortal.Domain.Processors.Interfaces;
using LazyCache;
using static System.Environment;
using System.Collections.Generic;

namespace AccountPortal.Domain.Processors
{
    public class ConsoleProcessor : IConsoleProcessor
    {
        private readonly ICacheProcessor _cacheProcessor;
        private readonly IAccountProcessor _accountProcessor;
        private readonly ITransactionProcessor _transactionProcessor;

        public ConsoleProcessor(ICacheProcessor cacheProcessor, IAccountProcessor accountProcessor, ITransactionProcessor transactionProcessor)
        {
            _cacheProcessor = cacheProcessor;
            _accountProcessor = accountProcessor;
            _transactionProcessor = transactionProcessor;
        }

        public void Execute()
        {
            var cache = _cacheProcessor.GetCache();
            GetRootMenu(cache);
        }

        private void GetRootMenu(IAppCache cache)
        {
            while (true)
            {
                var activeUser = new Account();
                switch (DisplayRootMenu())
                {
                    case 1:
                        activeUser = AddNewAccount(cache);
                        break;
                    case 2:
                        activeUser = LoginAccount(cache);
                        Console.WriteLine($"Welcome, {activeUser.Username}" + NewLine);
                        break;
                    default:
                        Console.WriteLine(NewLine + "Please make a valid selection." + NewLine);
                        DisplayRootMenu();
                        Console.ReadLine();
                        break;
                }

                if (!string.IsNullOrWhiteSpace(activeUser?.Username))
                {
                    GetTransactionMenu(cache, activeUser);
                }
                else if (activeUser?.Messages.Count > 0)
                {
                    activeUser.Messages.ForEach(x => Console.WriteLine(x + NewLine));
                    activeUser.Messages = new List<string>();
                    continue;
                }
                else
                {
                    Console.WriteLine("An unknown error has occurred.");
                    continue;
                }
                break;
            }
        }

        private void GetTransactionMenu(IAppCache cache, Account activeUser)
        {
            while (true)
            {
                var transactionMenu = DisplayTransactionMenu(activeUser.AccountBalance);
                switch (transactionMenu)
                {
                    case 1:
                        ShowHistoricalTransactions(activeUser);
                        continue;
                    case 2:
                        Console.WriteLine("Please enter a deposit amount: ");
                        var depositAmount = Console.ReadLine();
                        var depositResponse =_transactionProcessor.Deposit(activeUser, depositAmount);
                        if (depositResponse.Messages.Count > 0)
                        {
                            depositResponse.Messages.ForEach(x=> Console.WriteLine(x + NewLine));
                            activeUser.Messages = new List<string>();
                        }
                        continue;
                    case 3:
                        Console.WriteLine("Please enter an amount to withdraw: ");
                        var withdrawAmount = Console.ReadLine();
                        var withdrawResponse = _transactionProcessor.Withdraw(activeUser, withdrawAmount);
                        if (withdrawResponse.Messages.Count > 0)
                        {
                            withdrawResponse.Messages.ForEach(x => Console.WriteLine(x + NewLine));
                            activeUser.Messages = new List<string>();
                        }
                        continue;
                    case 4:
                        UpdateCache(cache,activeUser);
                        GetRootMenu(cache);
                        break;
                    default:
                        Console.WriteLine("Please make a valid selection." + NewLine);
                        DisplayTransactionMenu(activeUser.AccountBalance);
                        Console.ReadLine();
                        break;
                }
                break;
            }
        }

        private static void ShowHistoricalTransactions(Account activeUser)
        {
            if (activeUser.Transactions.Count > 0)
            {
                Console.WriteLine(NewLine + "Submitted date " + " --- " + "Amount" + " --- " + "Transaction type");
                foreach (var transaction in activeUser.Transactions)
                {
                    var transactionType = transaction.IsDeposit ? "Deposit" : "Withdrawl";
                    Console.WriteLine(transaction.SubmittedDate.ToLongDateString() + " --- " + transaction.Amount + " --- " + transactionType + NewLine);
                }
            }
            else
            {
                Console.WriteLine(NewLine + "No historical transactions." + NewLine);
            }
        }

        private static int DisplayRootMenu()
        {
            Console.WriteLine("Welcome to the Generic Account Portal. Please choose from the following options: " + NewLine);
            Console.WriteLine("1. Create new account" + NewLine);
            Console.WriteLine("2. Login to existing account" + NewLine);

            return ValidateMenuInput();
        }

        private static int ValidateMenuInput()
        {
            var input = Console.ReadLine();
            int.TryParse(input, out var output);
            return output;
        }

        private Account LoginAccount(IAppCache cache)
        {
            var account = new Account();

            Console.WriteLine(NewLine + "Please enter your user name: " + NewLine);
            account.Username = Console.ReadLine();
            Console.WriteLine("Please enter your password: " + NewLine);
            account.Password = Console.ReadLine();

            var getAccount = cache.Get<Account>(account.Username);
            return _accountProcessor.GetAccount(getAccount, account.Password);
        }

        private Account AddNewAccount(IAppCache cache)
        {
            var account = new Account();

            Console.WriteLine(NewLine + "Please choose a user name: " + NewLine);
            account.Username = Console.ReadLine();
            Console.WriteLine("Please choose a password: " + NewLine);
            account.Password = Console.ReadLine();
            var response =  _accountProcessor.AddAccount(account);
            if (response != null)
            {
                cache.Add(response.Username, response);
            }
            return response;
        }

        private static int DisplayTransactionMenu(decimal accountBalance)
        {
            Console.WriteLine($"Your account balance is: ${accountBalance}" + NewLine);
            Console.WriteLine("Please select from the following options: " + NewLine);
            Console.WriteLine("1. View Transaction History" + NewLine);
            Console.WriteLine("2. Deposit funds" + NewLine);
            Console.WriteLine("3. Withdraw funds" + NewLine);
            Console.WriteLine("4. Logout of current account" + NewLine);

            return ValidateMenuInput();
        }

        private static void UpdateCache(IAppCache cache, Account activeUser)
        {
            cache.Remove(activeUser.Username);
            cache.Add(activeUser.Username, activeUser);
        }
    }
}

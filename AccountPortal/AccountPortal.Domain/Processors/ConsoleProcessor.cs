using System;
using AccountPortal.data;
using AccountPortal.Domain.Extensions;
using AccountPortal.Domain.Models;
using AccountPortal.Domain.Processors.Interfaces;
using LazyCache;
using static System.Environment;

namespace AccountPortal.Domain.Processors
{
    public class ConsoleProcessor : IConsoleProcessor
    {
        private readonly ICacheRepository _cacheRepository;
        private readonly IAccountProcessor _accountProcessor;
        private readonly ITransactionProcessor _transactionProcessor;

        public ConsoleProcessor(ICacheRepository cacheRepository, IAccountProcessor accountProcessor, ITransactionProcessor transactionProcessor)
        {
            _cacheRepository = cacheRepository;
            _accountProcessor = accountProcessor;
            _transactionProcessor = transactionProcessor;
        }

        public void Execute()
        {
            var cache = _cacheRepository.GetCache();
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
                    continue;
                }
                else
                {
                    Console.WriteLine("An unknown error has occured.");
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
                        _transactionProcessor.Deposit(activeUser, depositAmount);
                        continue;
                    case 3:
                        Console.WriteLine("Please enter an amount to withdraw: ");
                        var withdrawAmount = Console.ReadLine();
                        _transactionProcessor.Withdraw(activeUser, withdrawAmount);
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

        public int DisplayRootMenu()
        {
            Console.WriteLine(NewLine + "Welcome to the Generic Account Portal. Please choose from the following options: " + NewLine);
            Console.WriteLine("1. Create new account" + NewLine);
            Console.WriteLine("2. Login to existing account" + NewLine);

            return ValidateMenuInput();
        }

        private static int ValidateMenuInput()
        {
            var output = 0;
            var input = Console.ReadLine();
            int.TryParse(input, out output);
            return output;
        }

        public Account LoginAccount(IAppCache cache)
        {
            var account = new Account();

            Console.WriteLine(NewLine + "Please enter your username: " + NewLine);
            account.Username = Console.ReadLine();
            Console.WriteLine("Please enter your password: " + NewLine);
            account.Password = Console.ReadLine();

            return _accountProcessor.GetAccount(cache,account);
        }

        public Account AddNewAccount(IAppCache cache)
        {
            var account = new Account();

            Console.WriteLine(NewLine + "Please choose a username: " + NewLine);
            account.Username = Console.ReadLine();
            Console.WriteLine("Please choose a password: " + NewLine);
            account.Password = Console.ReadLine();

            return _accountProcessor.AddAccount(cache, account);
        }

        public int DisplayTransactionMenu(decimal accountBalance)
        {
            Console.WriteLine(NewLine + $"Your account balance is: ${accountBalance}" + NewLine);
            Console.WriteLine("Please select from the following options: " + NewLine);
            Console.WriteLine("1. View Transaction History" + NewLine);
            Console.WriteLine("2. Deposit funds" + NewLine);
            Console.WriteLine("3. Withdraw funds" + NewLine);
            Console.WriteLine("4. Logout of current account" + NewLine);

            return ValidateMenuInput();
        }

        public void UpdateCache(IAppCache cache, Account activeUser)
        {
            cache.Remove(activeUser.Username);
            _accountProcessor.AddAccount(cache, activeUser);
        }
    }
}

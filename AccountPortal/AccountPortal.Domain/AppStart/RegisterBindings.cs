using AccountPortal.Data;
using AccountPortal.Domain.Extensions;
using AccountPortal.Domain.Processors;
using AccountPortal.Domain.Processors.Interfaces;
using LazyCache;
using Ninject;

namespace AccountPortal.Domain.AppStart
{
    public static class RegisterBindings
    {
        public static void RegisterConsole(IKernel kernel)
        {
            kernel.Bind<IAppCache>().To<CachingService>().InSingletonScope();
            kernel.Bind<IConsoleProcessor>().To<ConsoleProcessor>().InSingletonScope();
            kernel.Bind<ICacheRepository>().To<CacheRepository>().InSingletonScope();
            kernel.Bind<IEncryptionUtility>().To<EncryptionUtility>().InSingletonScope();
            kernel.Bind<ITransactionProcessor>().To<TransactionProcessor>().InSingletonScope();
            kernel.Bind<IAccountProcessor>().To<AccountProcessor>().InSingletonScope();
        }

        public static void RegisterWeb(IKernel kernel)
        {
            
        }
    }
}

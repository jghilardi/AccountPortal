using System.Reflection;
using AccountPortal.Domain.Processors.Interfaces;
using Ninject;

namespace AccountPortal
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var kernel = new StandardKernel();
            kernel.Load(Assembly.GetExecutingAssembly());
            var consoleApp = kernel.Get<IConsoleProcessor>();
            consoleApp.Execute();           
        }
    }
}

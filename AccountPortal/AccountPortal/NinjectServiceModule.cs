using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject.Modules;

namespace AccountPortal
{
    public class NinjectServiceModule : NinjectModule
    {
        public override void Load()
        {
            Domain.AppStart.RegisterBindings.RegisterConsole(Kernel);
        }
    }
}

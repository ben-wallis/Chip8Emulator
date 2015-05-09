using System;
using Castle.Windsor;
using Castle.Windsor.Installer;

namespace Chip8Emulator.Core
{
    class Program
    {
        static void Main(string[] args)
        {
            IWindsorContainer container = new WindsorContainer();
            
            container.Install(
                FromAssembly.This()
                );

            var emulatorShell = container.Resolve<IEmulatorShell>();
            emulatorShell.StartEmulation("c:\\chip8\\fishie.chip8");

            Console.ReadLine();
        }
    }
}

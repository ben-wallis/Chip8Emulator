using System;
using Castle.Windsor;
using Castle.Windsor.Installer;

namespace Chip8Emulator
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
            emulatorShell.RunEmulator();

            Console.ReadLine();
        }
    }
}

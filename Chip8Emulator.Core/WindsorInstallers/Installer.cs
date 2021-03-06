﻿using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace Chip8Emulator.Core.WindsorInstallers
{
    public class Installer : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component
                    .For<IEmulatorShell>()
                    .ImplementedBy<EmulatorShell>()
                    .LifestyleSingleton()
                );

            container.Register(
                Component
                    .For<ICpu>()
                    .ImplementedBy<Cpu>()
                    .LifestyleSingleton()
                );

            container.Register(
                Component
                    .For<IMemory>()
                    .ImplementedBy<Memory>()
                    .LifestyleSingleton()
                );

            container.Register(
                Component
                    .For<IDisassembler>()
                    .ImplementedBy<Disassembler>()
                    .LifestyleTransient()
                );

            container.Register(
                Component
                    .For<IFileHandler>()
                    .ImplementedBy<FileHandler>()
                    .LifestyleSingleton()
                );

            container.Register(
                Component
                    .For<IRegisterBank>()
                    .ImplementedBy<RegisterBank>()
                    .LifestyleSingleton()
                );

            container.Register(
                Component
                    .For<IDisplay>()
                    .ImplementedBy<Display>()
                    .LifestyleSingleton()
                );
        }
    }
}

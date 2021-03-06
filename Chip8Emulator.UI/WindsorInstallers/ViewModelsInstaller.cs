﻿using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Chip8Emulator.UI.ViewModels;

namespace Chip8Emulator.UI.WindsorInstallers
{
    public class ViewModelsInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component
                    .For<IMainWindowViewModel>()
                    .ImplementedBy<MainWindowViewModel>()
                    .LifestyleTransient()
                );

            container.Register(
                Component
                    .For<IEmulatorDisplayViewModel>()
                    .ImplementedBy<EmulatorDisplayViewModel>()
                    .LifestyleTransient()
                );

            container.Register(
                Component
                    .For<IEmulatorManagementViewModel>()
                    .ImplementedBy<EmulatorManagementViewModel>()
                    .LifestyleTransient()
                );

            container.Register(
                Component
                    .For<IEmulatorRegistersViewModel>()
                    .ImplementedBy<EmulatorRegistersViewModel>()
                    .LifestyleTransient()
                );
        }
    }
}

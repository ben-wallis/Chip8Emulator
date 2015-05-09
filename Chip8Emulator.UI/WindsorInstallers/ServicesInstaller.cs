using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Chip8Emulator.UI.Services;

namespace Chip8Emulator.UI.WindsorInstallers
{
    public class ServicesInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component
                                .For<IFolderBrowserDialogService>()
                                .ImplementedBy<FolderBrowserDialogService>()
                                .LifeStyle.Transient);

        }
    }
}

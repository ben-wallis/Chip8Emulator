using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Chip8Emulator.UI.Views;

namespace Chip8Emulator.UI.WindsorInstallers
{
    public class ViewsInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component
                                .For<IMainWindow>()
                                .ImplementedBy<MainWindow>()
                                .LifeStyle.Transient);

        }
    }
}

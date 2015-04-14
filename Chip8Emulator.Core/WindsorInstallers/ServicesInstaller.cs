using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Chip8Emulator.Core.Services;

namespace Chip8Emulator.Core.WindsorInstallers
{
    public class ServicesInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component
                    .For<IFileSystemService>()
                    .ImplementedBy<FileSystemService>()
                    .LifestyleSingleton()
                );

            container.Register(
                Component
                    .For<IRandomService>()
                    .ImplementedBy<RandomService>()
                    .LifestyleSingleton()
                );
        }
    }
}

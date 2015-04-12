using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Chip8Emulator.Services;

namespace Chip8Emulator.WindsorInstallers
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
        }
    }
}

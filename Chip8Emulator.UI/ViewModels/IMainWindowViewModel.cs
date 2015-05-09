using System.Windows.Input;

namespace Chip8Emulator.UI.ViewModels
{
    public interface IMainWindowViewModel
    {
        ICommand StartEmulationCommand { get; }
        ICommand StopEmulationCommand { get; }
        ICommand KeyDownCommand { get; }
        ICommand KeyUpCommand { get; }
        string RomFilePath { get; set; }
    }
}
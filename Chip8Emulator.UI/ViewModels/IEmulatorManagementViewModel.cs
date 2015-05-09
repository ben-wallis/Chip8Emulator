using System.ComponentModel;
using System.Windows.Input;

namespace Chip8Emulator.UI.ViewModels
{
    public interface IEmulatorManagementViewModel
    {
        string RomDirectoryPath { get; }
        ushort CycleDelay { get; }
        ICommand SelectRomDirectoryCommand { get; }
        event PropertyChangedEventHandler PropertyChanged;
    }
}
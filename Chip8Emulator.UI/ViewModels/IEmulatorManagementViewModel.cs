using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Media;

namespace Chip8Emulator.UI.ViewModels
{
    public interface IEmulatorManagementViewModel
    {
        string RomDirectoryPath { get; }
        ICommand SelectRomDirectoryCommand { get; }
        event PropertyChangedEventHandler PropertyChanged;
        void SelectRom(Visual sourceControl);
    }
}
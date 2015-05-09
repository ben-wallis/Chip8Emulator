using System.ComponentModel;

namespace Chip8Emulator.UI.ViewModels
{
    public interface IEmulatorRegistersViewModel
    {
        byte[] VRegisters { get; }
        event PropertyChangedEventHandler PropertyChanged;
    }
}
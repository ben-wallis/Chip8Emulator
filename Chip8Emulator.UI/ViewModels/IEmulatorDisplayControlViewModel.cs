using System.ComponentModel;

namespace Chip8Emulator.UI.ViewModels
{
    internal interface IEmulatorDisplayControlViewModel
    {
        void OnKeyDown(string key);
        void OnKeyUp(string key);
        bool[,] DisplayPixels { get; }
        event PropertyChangedEventHandler PropertyChanged;
    }
}
using System;
using System.ComponentModel;

namespace Chip8Emulator.UI.ViewModels
{
    internal interface IEmulatorDisplayControlViewModel
    {
        void OnDrawRequired(object sender, EventArgs e);
        bool[,] DisplayPixels { get; }
        event PropertyChangedEventHandler PropertyChanged;
    }
}
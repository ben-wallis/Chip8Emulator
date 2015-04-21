using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Chip8Emulator.Core;
using Chip8Emulator.UI.Annotations;

namespace Chip8Emulator.UI.ViewModels
{
    internal class EmulatorDisplayControlViewModel : INotifyPropertyChanged, IEmulatorDisplayControlViewModel
    {
        private readonly IEmulatorShell _emulatorShell;

        public EmulatorDisplayControlViewModel(IEmulatorShell emulatorShell)
        {
            _emulatorShell = emulatorShell;
            _emulatorShell.DrawRequired += OnDrawRequired;
        }

        public void OnDrawRequired(object sender, EventArgs e)
        {
            OnPropertyChanged("DisplayPixels");
        }

        public bool[,] DisplayPixels
        {
            get { return _emulatorShell.DisplayPixels; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

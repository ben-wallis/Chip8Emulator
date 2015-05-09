using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Chip8Emulator.Core;
using Chip8Emulator.UI.Annotations;

namespace Chip8Emulator.UI.ViewModels
{
    public class EmulatorRegistersViewModel : INotifyPropertyChanged, IEmulatorRegistersViewModel
    {
        private readonly IRegisterBank _registerBank;
        private readonly IEmulatorShell _emulatorShell;

        public EmulatorRegistersViewModel(IRegisterBank registerBank, IEmulatorShell emulatorShell)
        {
            _registerBank = registerBank;
            _emulatorShell = emulatorShell;
            _emulatorShell.CycleTick += OnCycleTick;
        }

        public byte[] VRegisters
        {
            get { return _registerBank.V; }
        }

        private void OnCycleTick(object sender, EventArgs e)
        {
            OnPropertyChanged("VRegisters");
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

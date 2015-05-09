using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Chip8Emulator.Core;
using Chip8Emulator.UI.Annotations;

namespace Chip8Emulator.UI.ViewModels
{
    internal class EmulatorDisplayViewModel : INotifyPropertyChanged, IEmulatorDisplayViewModel
    {
        private readonly IEmulatorShell _emulatorShell;

        public EmulatorDisplayViewModel(IEmulatorShell emulatorShell)
        {
            _emulatorShell = emulatorShell;
            _emulatorShell.DrawRequired += OnDrawRequired;
        }

        private void OnDrawRequired(object sender, EventArgs e)
        {
            OnPropertyChanged("DisplayPixels");
        }

        public void OnKeyDown(string key)
        {
            switch (key)
            {
                case "Numpad1":
                    _emulatorShell.OnKeyDown(0x7);
                    break;
                case "Numpad2":
                    _emulatorShell.OnKeyDown(0x8);
                    break;
                case "Numpad3":
                    _emulatorShell.OnKeyDown(0x9);
                    break;
                case "Numpad4":
                    _emulatorShell.OnKeyDown(0x4);
                    break;
                case "Numpad5":
                    _emulatorShell.OnKeyDown(0x5);
                    break;
                case "Numpad6":
                    _emulatorShell.OnKeyDown(0x6);
                    break;
                case "Numpad7":
                    _emulatorShell.OnKeyDown(0x1);
                    break;
                case "Numpad8":
                    _emulatorShell.OnKeyDown(0x2);
                    break;
                case "Numpad9":
                    _emulatorShell.OnKeyDown(0x3);
                    break;
                case "Numpad0":
                    _emulatorShell.OnKeyDown(0x0);
                    break;
                case "Divide":
                    _emulatorShell.OnKeyDown(0xa);
                    break;
                case "Multiply":
                    _emulatorShell.OnKeyDown(0xb);
                    break;
                case "Subtract":
                    _emulatorShell.OnKeyDown(0xc);
                    break;
                case "Add":
                    _emulatorShell.OnKeyDown(0xd);
                    break;
                case "Return":
                    _emulatorShell.OnKeyDown(0xe);
                    break;
                case "Decimal":
                    _emulatorShell.OnKeyDown(0xf);
                    break;
            }
        }

        public void OnKeyUp(string key)
        {
            switch (key)
            {
                case "Numpad1":
                    _emulatorShell.OnKeyUp(0x7);
                    break;
                case "Numpad2":
                    _emulatorShell.OnKeyUp(0x8);
                    break;
                case "Numpad3":
                    _emulatorShell.OnKeyUp(0x9);
                    break;
                case "Numpad4":
                    _emulatorShell.OnKeyUp(0x4);
                    break;
                case "Numpad5":
                    _emulatorShell.OnKeyUp(0x5);
                    break;
                case "Numpad6":
                    _emulatorShell.OnKeyUp(0x6);
                    break;
                case "Numpad7":
                    _emulatorShell.OnKeyUp(0x1);
                    break;
                case "Numpad8":
                    _emulatorShell.OnKeyUp(0x2);
                    break;
                case "Numpad9":
                    _emulatorShell.OnKeyUp(0x3);
                    break;
                case "Numpad0":
                    _emulatorShell.OnKeyUp(0x0);
                    break;
                case "Divide":
                    _emulatorShell.OnKeyUp(0xa);
                    break;
                case "Multiply":
                    _emulatorShell.OnKeyUp(0xb);
                    break;
                case "Subtract":
                    _emulatorShell.OnKeyUp(0xc);
                    break;
                case "Add":
                    _emulatorShell.OnKeyUp(0xd);
                    break;
                case "Return":
                    _emulatorShell.OnKeyUp(0xe);
                    break;
                case "Decimal":
                    _emulatorShell.OnKeyUp(0xf);
                    break;
            }
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

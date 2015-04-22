using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Input;
using Chip8Emulator.Core;
using GalaSoft.MvvmLight.CommandWpf;

namespace Chip8Emulator.UI.ViewModels
{
    internal class MainWindowViewModel : IMainWindowViewModel
    {
        private readonly IEmulatorShell _emulatorShell;
        private readonly IEmulatorDisplayControlViewModel _emulatorDisplayControlViewModel;
        private readonly Dictionary<string, bool> _keyRepeats = new Dictionary<string, bool>();

        public MainWindowViewModel(IEmulatorShell emulatorShell, IEmulatorDisplayControlViewModel emulatorDisplayControlViewModel)
        {
            _emulatorShell = emulatorShell;
            _emulatorDisplayControlViewModel = emulatorDisplayControlViewModel;
            StartEmulationCommand = new RelayCommand(StartEmulation);
            StopEmulationCommand = new RelayCommand(StopEmulation);
            KeyDownCommand = new RelayCommand<string>(OnKeyDown);
            KeyUpCommand = new RelayCommand<string>(OnKeyUp);
        }

        public ICommand StartEmulationCommand { get; private set; }
        public ICommand StopEmulationCommand { get; private set; }
        
        public ICommand KeyDownCommand { get; private set; }
        public ICommand KeyUpCommand { get; private set; }

        private void OnKeyDown(string key)
        {
            if (_keyRepeats.ContainsKey(key) && !_keyRepeats[key]) return;

            _keyRepeats[key] = false;
            Debug.WriteLine(key + " pressed");
            _emulatorDisplayControlViewModel.OnKeyDown(key);
        }

        private void OnKeyUp(string key)
        {
            _keyRepeats[key] = true;
            _emulatorDisplayControlViewModel.OnKeyUp(key);
        }

        public IEmulatorDisplayControlViewModel EmulatorDisplayControlViewModel
        {
            get
            {
                return _emulatorDisplayControlViewModel;
            }
        }

        private void StartEmulation()
        {
            _emulatorShell.StartEmulation();
        }

        private void StopEmulation()
        {
            _emulatorShell.StopEmulation();
        }

    }
}

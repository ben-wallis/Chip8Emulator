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
        private readonly IEmulatorDisplayViewModel _emulatorDisplayViewModel;
        private readonly IEmulatorManagementViewModel _emulatorManagementViewModel;
        private readonly Dictionary<string, bool> _keyRepeats = new Dictionary<string, bool>();

        public MainWindowViewModel(IEmulatorShell emulatorShell, IEmulatorDisplayViewModel emulatorDisplayViewModel, IEmulatorManagementViewModel emulatorManagementViewModel)
        {
            _emulatorShell = emulatorShell;
            _emulatorDisplayViewModel = emulatorDisplayViewModel;
            _emulatorManagementViewModel = emulatorManagementViewModel;
            StartEmulationCommand = new RelayCommand(StartEmulation);
            StopEmulationCommand = new RelayCommand(StopEmulation);
            KeyDownCommand = new RelayCommand<string>(OnKeyDown);
            KeyUpCommand = new RelayCommand<string>(OnKeyUp);
        }

        public ICommand StartEmulationCommand { get; private set; }
        public ICommand StopEmulationCommand { get; private set; }
        
        public ICommand KeyDownCommand { get; private set; }
        public ICommand KeyUpCommand { get; private set; }

        public string RomFilePath { get; set; }

        private void OnKeyDown(string key)
        {
            if (_keyRepeats.ContainsKey(key) && !_keyRepeats[key]) return;

            _keyRepeats[key] = false;
            Debug.WriteLine(key + " pressed");
            _emulatorDisplayViewModel.OnKeyDown(key);
        }

        private void OnKeyUp(string key)
        {
            _keyRepeats[key] = true;

            Debug.WriteLine(key + " up");
            _emulatorDisplayViewModel.OnKeyUp(key);
        }

        public IEmulatorDisplayViewModel EmulatorDisplayViewModel
        {
            get
            {
                return _emulatorDisplayViewModel;
            }
        }

        public IEmulatorManagementViewModel EmulatorManagementViewModel
        {
            get
            {
                return _emulatorManagementViewModel;
            }
        }

        private void StartEmulation()
        {
            _emulatorShell.StartEmulation(RomFilePath);
        }

        private void StopEmulation()
        {
            _emulatorShell.StopEmulation();
        }

    }
}

using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;

namespace Chip8Emulator.UI.ViewModels
{
    internal class MainWindowViewModel : IMainWindowViewModel
    {
        private readonly IEmulatorDisplayViewModel _emulatorDisplayViewModel;
        private readonly IEmulatorManagementViewModel _emulatorManagementViewModel;
        private readonly IEmulatorRegistersViewModel _emulatorRegistersViewModel;

        private readonly Dictionary<string, bool> _keyRepeats = new Dictionary<string, bool>();

        public MainWindowViewModel(IEmulatorDisplayViewModel emulatorDisplayViewModel, IEmulatorManagementViewModel emulatorManagementViewModel, IEmulatorRegistersViewModel emulatorRegistersViewModel)
        {
            _emulatorDisplayViewModel = emulatorDisplayViewModel;
            _emulatorManagementViewModel = emulatorManagementViewModel;
            _emulatorRegistersViewModel = emulatorRegistersViewModel;
            KeyDownCommand = new RelayCommand<string>(OnKeyDown);
            KeyUpCommand = new RelayCommand<string>(OnKeyUp);
        }

        public ICommand KeyDownCommand { get; private set; }
        public ICommand KeyUpCommand { get; private set; }

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

        public IEmulatorRegistersViewModel EmulatorRegistersViewModel
        {
            get
            {
                return _emulatorRegistersViewModel;
            }
        }
    }
}

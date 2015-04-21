using System.Windows.Input;
using Chip8Emulator.Core;
using GalaSoft.MvvmLight.CommandWpf;

namespace Chip8Emulator.UI.ViewModels
{
    internal class MainWindowViewModel : IMainWindowViewModel
    {
        private readonly IEmulatorShell _emulatorShell;
        private readonly IEmulatorDisplayControlViewModel _emulatorDisplayControlViewModel;

        public MainWindowViewModel(IEmulatorShell emulatorShell, IEmulatorDisplayControlViewModel emulatorDisplayControlViewModel)
        {
            _emulatorShell = emulatorShell;
            _emulatorDisplayControlViewModel = emulatorDisplayControlViewModel;
            StartEmulationCommand = new RelayCommand(StartEmulation);
            StopEmulationCommand = new RelayCommand(StopEmulation);
        }

        public ICommand StartEmulationCommand { get; private set; }
        public ICommand StopEmulationCommand { get; private set; }

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

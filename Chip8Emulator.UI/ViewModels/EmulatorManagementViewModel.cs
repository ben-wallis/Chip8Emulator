using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using Chip8Emulator.Core;
using Chip8Emulator.UI.Annotations;
using Chip8Emulator.UI.Services;
using GalaSoft.MvvmLight.CommandWpf;

namespace Chip8Emulator.UI.ViewModels
{
    public class EmulatorManagementViewModel : INotifyPropertyChanged, IEmulatorManagementViewModel
    {
        private readonly IFolderBrowserDialogService _folderBrowserDialogService;
        private readonly IEmulatorShell _emulatorShell;

        private string _romDirectoryPath;
        private bool _emulationRunning;
        private ushort _cycleDelay;

        public EmulatorManagementViewModel(IFolderBrowserDialogService folderBrowserDialogService, IEmulatorShell emulatorShell)
        {
            _folderBrowserDialogService = folderBrowserDialogService;
            _emulatorShell = emulatorShell;
            _emulatorShell.EmulationStopped += EmulationStopped;

            SelectRomDirectoryCommand = new RelayCommand<Visual>(SelectRom);
            StartEmulationCommand = new RelayCommand(StartEmulation, () => !EmulationRunning);
            StopEmulationCommand = new RelayCommand(StopEmulation, () => EmulationRunning);

            RomFilename = "fishie.chip8";
            RomDirectoryPath = "C:\\chip8";
            CycleDelay = 10;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand SelectRomDirectoryCommand { get; private set; }
        public ICommand StartEmulationCommand { get; private set; }
        public ICommand StopEmulationCommand { get; private set; }

        public string RomDirectoryPath
        {
            get { return _romDirectoryPath; }
            private set
            {
                _romDirectoryPath = value;
                OnPropertyChanged();
            }
        }

        public string RomFilename { get; set; }

        public bool EmulationRunning
        {
            get { return _emulationRunning; }
            set
            {
                _emulationRunning = value; 
                OnPropertyChanged();
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public ushort CycleDelay
        {
            get { return _cycleDelay; }
            set
            {
                _cycleDelay = value;
                _emulatorShell.CycleDelay = value;
                OnPropertyChanged();
            }
        }

        private void EmulationStopped(object sender, EventArgs e)
        {
            EmulationRunning = false;
        }

        private void SelectRom(Visual sourceControl)
        {
            var result = _folderBrowserDialogService.ShowDialog(sourceControl);
            
            if (result != DialogResult.OK) return;

            RomDirectoryPath = _folderBrowserDialogService.SelectedFolderPath;
        }

        private void StartEmulation()
        {
            EmulationRunning = true;
            try
            {
                _emulatorShell.StartEmulation(RomDirectoryPath + "\\" + RomFilename);
            }
            catch (Exception e)
            {
                MessageBox.Show(@"Exception occurred in emulator: " + e.Message, @"Emulation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                EmulationRunning = false;
            }
        }

        private void StopEmulation()
        {
            _emulatorShell.StopEmulation();
            EmulationRunning = false;
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

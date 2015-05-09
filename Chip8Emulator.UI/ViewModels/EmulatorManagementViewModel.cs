using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using Chip8Emulator.UI.Annotations;
using Chip8Emulator.UI.Extensions;
using GalaSoft.MvvmLight.Command;

namespace Chip8Emulator.UI.ViewModels
{
    public class EmulatorManagementViewModel : INotifyPropertyChanged, IEmulatorManagementViewModel
    {
        private string _romDirectoryPath;

        public EmulatorManagementViewModel()
        {
            SelectRomDirectoryCommand = new RelayCommand<Visual>(SelectRom);
        }

        public string RomDirectoryPath
        {
            get { return _romDirectoryPath; }
            private set
            {
                _romDirectoryPath = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand SelectRomDirectoryCommand { get; private set; }

        public void SelectRom(Visual sourceControl)
        {
            var dlg = new FolderBrowserDialog();
            var result = dlg.ShowDialog(sourceControl.GetIWin32Window());
            
            if (result != DialogResult.OK) return;

            RomDirectoryPath = dlg.SelectedPath;
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

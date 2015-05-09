using System.Windows.Forms;
using System.Windows.Media;
using Chip8Emulator.UI.Extensions;

namespace Chip8Emulator.UI.Services
{
    class FolderBrowserDialogService : IFolderBrowserDialogService
    {
        public string SelectedFolderPath { get; private set; }

        public DialogResult ShowDialog(Visual sourceControl)
        {
            var dlg = new FolderBrowserDialog();
            var result = dlg.ShowDialog(sourceControl.GetIWin32Window());
            SelectedFolderPath = dlg.SelectedPath;
            return result;
        }
    }
}

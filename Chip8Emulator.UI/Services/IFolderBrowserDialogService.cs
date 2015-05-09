using System.Windows.Forms;
using System.Windows.Media;

namespace Chip8Emulator.UI.Services
{
    public interface IFolderBrowserDialogService
    {
        DialogResult ShowDialog(Visual sourceControl);
        string SelectedFolderPath { get; }
    }
}
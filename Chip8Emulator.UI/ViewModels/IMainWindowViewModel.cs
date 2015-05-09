using System.Windows.Input;

namespace Chip8Emulator.UI.ViewModels
{
    public interface IMainWindowViewModel
    {
        ICommand KeyDownCommand { get; }
        ICommand KeyUpCommand { get; }
    }
}
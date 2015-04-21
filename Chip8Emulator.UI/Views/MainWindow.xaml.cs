using Chip8Emulator.UI.ViewModels;

namespace Chip8Emulator.UI.Views
{
    public partial class MainWindow : IMainWindow
    {
        public MainWindow(IMainWindowViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}

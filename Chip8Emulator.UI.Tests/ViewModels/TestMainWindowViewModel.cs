using Chip8Emulator.Core;
using Chip8Emulator.UI.ViewModels;
using Moq;
using NUnit.Framework;

namespace Chip8Emulator.UI.Tests.ViewModels
{
    [TestFixture]
    public class TestMainWindowViewModel
    {
        private MainWindowViewModelTestUtility _testUtility;

        [SetUp]
        public void MainWindowViewModelTestSetup()
        {
            _testUtility = new MainWindowViewModelTestUtility();
        }
        
        [Test]
        public void StartEmulationCommand_CallsEmulatorShellStartEmulation()
        {
            // Arrange
            _testUtility.MockEmulatorShell.Setup(s => s.StartEmulation()).Verifiable();

            // Act
            _testUtility.TestMainWindowViewModel.StartEmulationCommand.Execute(null);
            
            // Assert
            _testUtility.MockEmulatorShell.Verify(s => s.StartEmulation());
        }

        [Test]
        public void StopEmulationCommand_CallsEmulatorShellStopEmulation()
        {
            // Arrange
            _testUtility.MockEmulatorShell.Setup(s => s.StopEmulation()).Verifiable();

            // Act
            _testUtility.TestMainWindowViewModel.StopEmulationCommand.Execute(null);

            // Assert
            _testUtility.MockEmulatorShell.Verify(s => s.StopEmulation());
        }

        private class MainWindowViewModelTestUtility
        {
            public MainWindowViewModelTestUtility()
            {
                // Mock setups
                MockEmulatorShell = new Mock<IEmulatorShell>();
                MockEmulatorDisplayControlViewModel = new Mock<IEmulatorDisplayControlViewModel>();

                //Class under test instantiation
                TestMainWindowViewModel = new MainWindowViewModel(MockEmulatorShell.Object, MockEmulatorDisplayControlViewModel.Object);
            }
            public Mock<IEmulatorShell> MockEmulatorShell { get; private set; }
            public Mock<IEmulatorDisplayControlViewModel> MockEmulatorDisplayControlViewModel { get; private set; }
            public MainWindowViewModel TestMainWindowViewModel { get; private set; }
        }
    }
}

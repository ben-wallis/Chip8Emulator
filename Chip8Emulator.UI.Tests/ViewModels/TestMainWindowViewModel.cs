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
        public void StartEmulationCommand_CallsEmulatorShellStartEmulationWithRomFilePath()
        {
            // Arrange
            _testUtility.TestMainWindowViewModel.RomFilePath = "C:\\Testfile.txt";
            _testUtility.MockEmulatorShell.Setup(s => s.StartEmulation(_testUtility.TestMainWindowViewModel.RomFilePath)).Verifiable();

            // Act
            _testUtility.TestMainWindowViewModel.StartEmulationCommand.Execute(null);
            
            // Assert
            _testUtility.MockEmulatorShell.Verify(s => s.StartEmulation(_testUtility.TestMainWindowViewModel.RomFilePath));
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

        [Test]
        public void OnKeyDownCommand_CallsEmulatorDisplayControlViewModelOnKeyDown()
        {
            // Arrange
            const string TestInputKey = "Numpad4";
            _testUtility.MockEmulatorDisplayViewModel.Setup(v => v.OnKeyDown(TestInputKey)).Verifiable();

            // Act
            _testUtility.TestMainWindowViewModel.KeyDownCommand.Execute(TestInputKey);

            // Assert
            _testUtility.MockEmulatorDisplayViewModel.Verify(v => v.OnKeyDown(TestInputKey));
        }

        [Test]
        public void OnKeyUpCommand_CallsEmulatorDisplayControlViewModelOnKeyUp()
        {
            // Arrange
            const string TestInputKey = "Numpad4";
            _testUtility.MockEmulatorDisplayViewModel.Setup(v => v.OnKeyUp(TestInputKey)).Verifiable();

            // Act
            _testUtility.TestMainWindowViewModel.KeyUpCommand.Execute(TestInputKey);

            // Assert
            _testUtility.MockEmulatorDisplayViewModel.Verify(v => v.OnKeyUp(TestInputKey));
        }

        [Test]
        public void OnKeyDownCommand_DoesntSendKeyDownIfNoKeyUpSent()
        {
            // Arrange
            const string TestInputKey = "Numpad4";
            _testUtility.MockEmulatorDisplayViewModel.Setup(v => v.OnKeyDown(TestInputKey)).Verifiable();

            // Act
            _testUtility.TestMainWindowViewModel.KeyDownCommand.Execute(TestInputKey);
            _testUtility.TestMainWindowViewModel.KeyDownCommand.Execute(TestInputKey);

            // Assert
            _testUtility.MockEmulatorDisplayViewModel.Verify(v => v.OnKeyDown(TestInputKey), Times.Once);
        }

        [Test]
        public void OnKeyDownCommand_SendsKeyDownIfKeyUpReceivedSinceLastKeyDown()
        {
            // Arrange
            const string TestInputKey = "Numpad4";
            _testUtility.MockEmulatorDisplayViewModel.Setup(v => v.OnKeyDown(TestInputKey)).Verifiable();

            // Act
            _testUtility.TestMainWindowViewModel.KeyDownCommand.Execute(TestInputKey);
            _testUtility.TestMainWindowViewModel.KeyUpCommand.Execute(TestInputKey);
            _testUtility.TestMainWindowViewModel.KeyDownCommand.Execute(TestInputKey);

            // Assert
            _testUtility.MockEmulatorDisplayViewModel.Verify(v => v.OnKeyDown(TestInputKey), Times.Exactly(2));
        }

        private class MainWindowViewModelTestUtility
        {
            public MainWindowViewModelTestUtility()
            {
                // Mock setups
                MockEmulatorShell = new Mock<IEmulatorShell>();
                MockEmulatorDisplayViewModel = new Mock<IEmulatorDisplayViewModel>();
                
                TestRomFilePath = "C:\\testfile.txt";
                //Class under test instantiation
                TestMainWindowViewModel = new MainWindowViewModel(MockEmulatorShell.Object, MockEmulatorDisplayViewModel.Object, MockEmulatorManagementViewModel.Object);
            }
            public Mock<IEmulatorShell> MockEmulatorShell { get; private set; }
            public Mock<IEmulatorDisplayViewModel> MockEmulatorDisplayViewModel { get; private set; }
            public Mock<IEmulatorManagementViewModel> MockEmulatorManagementViewModel { get; private set; }
            public MainWindowViewModel TestMainWindowViewModel { get; private set; }
            public string TestRomFilePath { get; private set; }
        }
    }
}

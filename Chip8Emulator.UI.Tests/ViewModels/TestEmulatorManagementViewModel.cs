using System;
using System.Windows.Forms;
using Chip8Emulator.Core;
using Chip8Emulator.UI.Services;
using Chip8Emulator.UI.ViewModels;
using Moq;
using NUnit.Framework;
using UserControl = System.Windows.Controls.UserControl;

namespace Chip8Emulator.UI.Tests.ViewModels
{
    [TestFixture]
    public class TestEmulatorManagementViewModel
    {
        private EmulatorManagementViewModelTestUtility _testUtility;

        [SetUp]
        public void EmulatorManagementViewModelTestSetup()
        {
            _testUtility = new EmulatorManagementViewModelTestUtility();
        }

        [Test]
        [STAThread]
        public void SelectRomDirectoryCommand_CallsFolderBrowserDialogService()
        {
            // Arrange
            var testInputVisual = new UserControl();

            _testUtility.MockFolderBrowserDialogService.Setup(f => f.ShowDialog(testInputVisual)).Verifiable();
            
            // Act
            _testUtility.ViewModel.SelectRomDirectoryCommand.Execute(testInputVisual);
            
            // Assert
            _testUtility.MockFolderBrowserDialogService.Verify(f => f.ShowDialog(testInputVisual));
        }

        [Test]
        [STAThread]
        public void SelectRomDirectoryCommand_DialogResultOK_SetsRomDirectoryPath()
        {
            // Arrange
            var testInputVisual = new UserControl();

            const string TestInputSelectedFolder = "C:\\TestFolder";
            _testUtility.MockFolderBrowserDialogService.SetupGet(f => f.SelectedFolderPath)
                .Returns(TestInputSelectedFolder);
            _testUtility.MockFolderBrowserDialogService.Setup(f => f.ShowDialog(testInputVisual))
                .Returns(DialogResult.OK);

            // Act
            _testUtility.ViewModel.SelectRomDirectoryCommand.Execute(testInputVisual);

            // Assert
            Assert.AreEqual(TestInputSelectedFolder, _testUtility.ViewModel.RomDirectoryPath);
        }

        [Test]
        public void CycleDelay_Set_UpdatesEmulatorShellCycleDelay()
        {
            // Arrange
            const ushort TestCycleDelay = 15;
            _testUtility.MockEmulatorShell.SetupSet(e => e.CycleDelay = TestCycleDelay).Verifiable();

            // Act
            _testUtility.ViewModel.CycleDelay = TestCycleDelay;

            // Assert
            _testUtility.MockEmulatorShell.Verify();
        }

        [Test]
        [STAThread]
        public void SelectRomDirectoryCommand_DialogResultNotOK_DoesNotSetRomDirectoryPath()
        {
            // Arrange
            var testInputVisual = new UserControl();

            const string TestInputExistingSelectedFolder = "C:\\ExistingFolder";
            const string TestInputSelectedFolder = "C:\\TestFolder";
            
            _testUtility.MockFolderBrowserDialogService.SetupGet(f => f.SelectedFolderPath)
                .Returns(TestInputExistingSelectedFolder);
            _testUtility.MockFolderBrowserDialogService.Setup(f => f.ShowDialog(testInputVisual))
                .Returns(DialogResult.OK);
            _testUtility.ViewModel.SelectRomDirectoryCommand.Execute(testInputVisual);

            _testUtility.MockFolderBrowserDialogService.SetupGet(f => f.SelectedFolderPath)
                .Returns(TestInputSelectedFolder);
            _testUtility.MockFolderBrowserDialogService.Setup(f => f.ShowDialog(testInputVisual))
                .Returns(DialogResult.Cancel);

            // Act
            _testUtility.ViewModel.SelectRomDirectoryCommand.Execute(testInputVisual);

            // Assert
            Assert.AreEqual(TestInputExistingSelectedFolder, _testUtility.ViewModel.RomDirectoryPath);
        }

        [Test]
        [STAThread]
        public void StartEmulationCommand_CallsEmulatorShellStartEmulationWithRomFilePath()
        {
            // Arrange
            var testInputVisual = new UserControl();
            const string TestInputRomFilename = "Test.rom";
            const string TestInputRomDirectory = "C:\\TestDirectory";
            const string ExpectedRomPath = TestInputRomDirectory + "\\" + TestInputRomFilename;
            
            _testUtility.ViewModel.RomFilename = TestInputRomFilename;
            _testUtility.MockFolderBrowserDialogService.SetupGet(f => f.SelectedFolderPath)
                .Returns(TestInputRomDirectory);
            _testUtility.MockFolderBrowserDialogService.Setup(f => f.ShowDialog(testInputVisual))
                .Returns(DialogResult.OK);
            _testUtility.ViewModel.SelectRomDirectoryCommand.Execute(testInputVisual);
            
            _testUtility.MockEmulatorShell.Setup(s => s.StartEmulation(ExpectedRomPath)).Verifiable();

            // Act
            _testUtility.ViewModel.StartEmulationCommand.Execute(null);

            // Assert
            _testUtility.MockEmulatorShell.Verify(s => s.StartEmulation(ExpectedRomPath));
        }

        [Test]
        public void StartEmulationCommand_SetsEmulationRunningTrue()
        {
            // Arrange

            // Act
            _testUtility.ViewModel.StartEmulationCommand.Execute(null);

            // Assert
            Assert.IsTrue(_testUtility.ViewModel.EmulationRunning);
        }

        [Test]
        public void StopEmulationCommand_SetsEmulationRunningFalse()
        {
            // Arrange
            _testUtility.ViewModel.EmulationRunning = true;

            // Act
            _testUtility.ViewModel.StopEmulationCommand.Execute(null);

            // Assert
            Assert.IsFalse(_testUtility.ViewModel.EmulationRunning);
        }

        [Test]
        public void StopEmulationCommand_CallsEmulatorShellStopEmulation()
        {
            // Arrange
            _testUtility.MockEmulatorShell.Setup(s => s.StopEmulation()).Verifiable();
            _testUtility.ViewModel.EmulationRunning = true;

            // Act
            _testUtility.ViewModel.StopEmulationCommand.Execute(null);

            // Assert
            _testUtility.MockEmulatorShell.Verify(s => s.StopEmulation());
        }

        [Test]
        public void EmulationStoppedEvent_Fired_SetsEmulationRunningFalse()
        {
            // Arrange
            _testUtility.ViewModel.EmulationRunning = true;

            // Act
            _testUtility.MockEmulatorShell.Raise(e => e.EmulationStopped += null, new EventArgs());

            // Assert
            Assert.AreEqual(false, _testUtility.ViewModel.EmulationRunning);
        }

        private class EmulatorManagementViewModelTestUtility
        {
            public EmulatorManagementViewModelTestUtility()
            {
                // Mock Setups
                MockFolderBrowserDialogService = new Mock<IFolderBrowserDialogService>();
                MockEmulatorShell = new Mock<IEmulatorShell>();

                // Class under test instantiation
                ViewModel = new EmulatorManagementViewModel(MockFolderBrowserDialogService.Object, MockEmulatorShell.Object);
            }

            public Mock<IFolderBrowserDialogService> MockFolderBrowserDialogService { get; private set; }
            public Mock<IEmulatorShell> MockEmulatorShell { get; private set; }
            public EmulatorManagementViewModel ViewModel { get; private set; }
        }
    }
}

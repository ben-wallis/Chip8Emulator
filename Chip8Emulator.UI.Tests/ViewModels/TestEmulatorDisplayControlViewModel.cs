using Chip8Emulator.Core;
using Chip8Emulator.UI.ViewModels;
using Moq;
using NUnit.Framework;

namespace Chip8Emulator.UI.Tests.ViewModels
{
    [TestFixture]
    public class TestEmulatorDisplayControlViewModel
    {
        private EmulatorDisplayControlViewModelTestUtility _testUtility;

        [SetUp]
        public void TestSetup()
        {
            _testUtility = new EmulatorDisplayControlViewModelTestUtility();
        }

        [Test]
        public void DisplayPixels_ReturnsDisplayPixelsFromEmulatorShell()
        {
            // Arrange
            var testPixels = new bool[1, 2];
            _testUtility.MockEmulatorShell.Setup(e => e.DisplayPixels).Returns(testPixels);

            // Act
            var pixels = _testUtility.TestViewModel.DisplayPixels;

            // Assert
            Assert.AreSame(testPixels, pixels);
        }

        [TestCase("Numpad1", 0x7)]
        [TestCase("Numpad2", 0x8)]
        [TestCase("Numpad3", 0x9)]
        [TestCase("Numpad4", 0x4)]
        [TestCase("Numpad5", 0x5)]
        [TestCase("Numpad6", 0x6)]
        [TestCase("Numpad7", 0x1)]
        [TestCase("Numpad8", 0x2)]
        [TestCase("Numpad9", 0x3)]
        [TestCase("Numpad0", 0x0)]
        [TestCase("Divide", 0xa)]
        [TestCase("Multiply", 0xb)]
        [TestCase("Subtract", 0xc)]
        [TestCase("Add", 0xd)]
        [TestCase("Return", 0xe)]
        [TestCase("Decimal", 0xf)]
        public void OnKeyDown_CallsEmulatorShellOnKeyDownWithCorrectKeyCode(string key, byte expectedValue)
        {
            // Arrange
            _testUtility.MockEmulatorShell.Setup(s => s.OnKeyDown(expectedValue)).Verifiable();

            // Act
            _testUtility.TestViewModel.OnKeyDown(key);

            // Assert
            _testUtility.MockEmulatorShell.Verify(s => s.OnKeyDown(expectedValue));
        }

        [TestCase("Numpad1", 0x7)]
        [TestCase("Numpad2", 0x8)]
        [TestCase("Numpad3", 0x9)]
        [TestCase("Numpad4", 0x4)]
        [TestCase("Numpad5", 0x5)]
        [TestCase("Numpad6", 0x6)]
        [TestCase("Numpad7", 0x1)]
        [TestCase("Numpad8", 0x2)]
        [TestCase("Numpad9", 0x3)]
        [TestCase("Numpad0", 0x0)]
        [TestCase("Divide", 0xa)]
        [TestCase("Multiply", 0xb)]
        [TestCase("Subtract", 0xc)]
        [TestCase("Add", 0xd)]
        [TestCase("Return", 0xe)]
        [TestCase("Decimal", 0xf)]
        public void OnKeyUp_CallsEmulatorShellOnKeyUpWithCorrectKeyCode(string key, byte expectedValue)
        {
            // Arrange
            _testUtility.MockEmulatorShell.Setup(s => s.OnKeyUp(expectedValue)).Verifiable();

            // Act
            _testUtility.TestViewModel.OnKeyUp(key);

            // Assert
            _testUtility.MockEmulatorShell.Verify(s => s.OnKeyUp(expectedValue));
        }

        private class EmulatorDisplayControlViewModelTestUtility
        {
            public EmulatorDisplayControlViewModelTestUtility()
            {
                // Mock setups
                MockEmulatorShell = new Mock<IEmulatorShell>();

                //Class under test instantiation
                TestViewModel = new EmulatorDisplayViewModel(MockEmulatorShell.Object);
            }
            public Mock<IEmulatorShell> MockEmulatorShell { get; private set; }

            public EmulatorDisplayViewModel TestViewModel { get; private set; }
        }
    }
}

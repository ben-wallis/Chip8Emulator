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

        private class EmulatorDisplayControlViewModelTestUtility
        {
            public EmulatorDisplayControlViewModelTestUtility()
            {
                // Mock setups
                MockEmulatorShell = new Mock<IEmulatorShell>();

                //Class under test instantiation
                TestViewModel = new EmulatorDisplayControlViewModel(MockEmulatorShell.Object);
            }
            public Mock<IEmulatorShell> MockEmulatorShell { get; private set; }

            public EmulatorDisplayControlViewModel TestViewModel { get; private set; }
        }
    }
}

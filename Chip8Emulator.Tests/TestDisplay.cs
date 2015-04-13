using NUnit.Framework;

namespace Chip8Emulator.Core.Core.Tests
{
    // TODO: Tidy up tests, add test utility to remove the need to new up Display in every test
    [TestFixture]
    public class TestDisplay
    {
        [Test]
        public void Constructor_InitialisesDisplayToCorrectSize()
        {
            // Arrange
            
            // Act
            var display = new Display();

            // Assert
            Assert.AreEqual(64, display.Pixels.GetLength(0));
            Assert.AreEqual(32, display.Pixels.GetLength(1));
        }

        [Test]
        public void DumpToConsole_PrintsDisplayToConsole()
        {
            // Arrange
            var display = new Display();

            // Act
            display.DumpToConsole();

            // Assert
        }

        [Test]
        public void FlipPixel_SetsPixelToTrueIfAlreadyFalse()
        {
            // Arrange
            const byte TestX = 40;
            const byte TestY = 20;

            var display = new Display();

            // Act
            display.FlipPixel(TestX, TestY);

            // Assert
            Assert.AreEqual(true, display.Pixels[TestX, TestY]);
        }

        [Test]
        public void FlipPixel_SetsPixelToFalseIfAlreadyTrue()
        {
            // Arrange
            var display = new Display();
            display.FlipPixel(10, 10);

            // Act
            display.FlipPixel(10, 10);

            // Assert
            Assert.AreEqual(false, display.Pixels[10, 10]);
        }

        [Test]
        public void FlipPixel_ReturnsTrueIfBitFlippedToFalseFromTrue()
        {
            // Arrange
            var display = new Display();
            display.FlipPixel(10, 10);

            // Act
            var result = display.FlipPixel(10, 10);

            // Assert
            Assert.AreEqual(true, result);
        }

        [Test]
        public void FlipPixel_ReturnsFalseIfBitNotFlippedToFalseFromTrue()
        {
            // Arrange
            var display = new Display();

            // Act
            var result = display.FlipPixel(10, 10);

            // Assert
            Assert.AreEqual(false, result);
        }

        [Test]
        public void Initialise_SetsAllPixelsToFalse()
        {
            // Arrange
            var display = new Display();
            display.FlipPixel(10, 10);
            display.FlipPixel(63, 31);

            // Act
            display.Initialise();

            // Assert
            Assert.AreEqual(false, display.Pixels[10, 10]);
            Assert.AreEqual(false, display.Pixels[63, 31]);
        }
    }
}

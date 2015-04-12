using System;
using NUnit.Framework;

namespace Chip8Emulator.Tests
{
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
        public void SetPixelValue_SetsPixelToValue()
        {
            // Arrange
            const byte TestX = 40;
            const byte TestY = 20;

            var display = new Display();

            // Act
            display.SetPixel(TestX, TestY, true);

            // Assert
            Assert.AreEqual(true, display.Pixels[TestX, TestY]);
        }

        [Test]
        public void Initialise_SetsAllPixelsToFalse()
        {
            // Arrange
            var display = new Display();
            display.SetPixel(10, 10, true);
            display.SetPixel(63, 31, true);

            // Act
            display.Initialise();

            // Assert
            Assert.AreEqual(false, display.Pixels[10, 10]);
            Assert.AreEqual(false, display.Pixels[63, 31]);
        }
    }
}

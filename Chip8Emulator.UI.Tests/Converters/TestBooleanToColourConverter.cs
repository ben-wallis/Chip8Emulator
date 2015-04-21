using System.Windows.Media;
using Chip8Emulator.UI.Converters;
using NUnit.Framework;

namespace Chip8Emulator.UI.Tests.Converters
{
    [TestFixture]
    public class TestBooleanToColourConverter
    {
        [Test]
        public void Convert_True_ReturnsBlack()
        {
            // Arrange
            var converter = new BooleanToColourConverter();

            // Act
            var result = (SolidColorBrush)converter.Convert(true, null, null, null);

            // Assert
            Assert.AreEqual(result.Color, Colors.Black);
        }

        [Test]
        public void Convert_False_ReturnsWhite()
        {
            // Arrange
            var converter = new BooleanToColourConverter();

            // Act
            var result = (SolidColorBrush)converter.Convert(false, null, null, null);

            // Assert
            Assert.AreEqual(result.Color, Colors.White);
        }
    }
}

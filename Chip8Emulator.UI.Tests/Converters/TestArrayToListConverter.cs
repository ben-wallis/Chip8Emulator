using System.Collections.Generic;
using System.Linq;
using Chip8Emulator.UI.Converters;
using NUnit.Framework;

namespace Chip8Emulator.UI.Tests.Converters
{
    [TestFixture]
    public class TestArrayToListConverter
    {
        [Test]
        public void Convert_ReturnsCorrectlySizedListOfLists()
        {
            // Arrange
            const int TestArrayWidth = 5;
            const int TestArrayHeight = 10;
            var testArray = new bool[TestArrayWidth, TestArrayHeight];

            var converter = new ArrayToListConverter();
            
            // Act
            var result = (List<List<bool>>)converter.Convert(testArray, null, null, null);

            // Assert
            Assert.AreEqual(TestArrayHeight, result.Count);
            var firstListItem = result.First();
            Assert.AreEqual(TestArrayWidth, firstListItem.Count);
        }
    }
}

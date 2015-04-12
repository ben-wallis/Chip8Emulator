using NUnit.Framework;

namespace Chip8Emulator.Tests
{
    [TestFixture]
    public class TestMemory
    {
        [Test]
        public void GetValue_ReturnsValueFromSpecifiedAddress()
        {
            // Arrange
            const ushort TestInputAddress = 0x200;
            const byte TestInputValue = 0xF;

            var memory = new Memory();
            memory.SetValue(TestInputAddress, TestInputValue);

            // Act
            var result = memory.GetValue(TestInputAddress);

            // Assert
            Assert.AreEqual(TestInputValue, result);
        }

        [Test]
        public void LoadProgram_CopiesProgramToCorrectAddress()
        {
            // Arrange
            const byte TestByte1 = 0x1;
            const byte TestByte2 = 0x2;
            const byte TestByte3 = 0x3;
            var testInputProgram = new[] {TestByte1, TestByte2, TestByte3};

            // Act
            var memory = new Memory();
            memory.LoadProgram(testInputProgram);

            // Assert
            var address1 = memory.GetValue(0x200);
            var address2 = memory.GetValue(0x201);
            var address3 = memory.GetValue(0x202);

            Assert.AreEqual(TestByte1, address1);
            Assert.AreEqual(TestByte2, address2);
            Assert.AreEqual(TestByte3, address3);
        }
    }
}

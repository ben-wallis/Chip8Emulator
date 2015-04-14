using NUnit.Framework;

namespace Chip8Emulator.Core.Tests
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
            
            var memory = new Memory();

            // Act
            memory.LoadProgram(testInputProgram);

            // Assert
            var address1 = memory.GetValue(0x200);
            var address2 = memory.GetValue(0x201);
            var address3 = memory.GetValue(0x202);

            Assert.AreEqual(TestByte1, address1);
            Assert.AreEqual(TestByte2, address2);
            Assert.AreEqual(TestByte3, address3);
        }

        [Test]
        public void Reset_ClearsMemory()
        {
            // Arrange
            const byte TestData = 0xff;
            const ushort TestAddress = 0x200;

            var memory = new Memory();
            memory.SetValue(TestAddress, TestData);

            // Act
            memory.Reset();

            // Assert
            Assert.AreEqual(0x00, memory.GetValue(TestAddress));
        }
    }
}

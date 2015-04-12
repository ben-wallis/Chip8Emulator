using Moq;
using NUnit.Framework;

namespace Chip8Emulator.Tests
{
    [TestFixture]
    public class TestCpu
    {
        [Test]
        public void Constructor_InitialisesRegisters()
        {
            // Arrange
            var mockMemory = new Mock<IMemory>();
            var cpu = new Cpu(mockMemory.Object);

            // Act
            
            // Assert
            Assert.AreEqual(16, cpu.V.Length);
        }

        [Test]
        public void Constructor_InitialisesProgramCounter()
        {
            // Arrange
            var mockMemory = new Mock<IMemory>();
            var cpu = new Cpu(mockMemory.Object);

            // Act
            
            // Assert
            Assert.AreEqual(0x200, cpu.PC);
        }
        
        [Test]
        public void Constructor_InitialisesStackPointer()
        {
            // Arrange
            var mockMemory = new Mock<IMemory>();
            var cpu = new Cpu(mockMemory.Object);

            // Act

            // Assert
            Assert.AreEqual(0xfa0, cpu.SP);
        }

        [Test]
        public void EmulateOp_1NNN_MovesProgramCounterToAddress()
        {
            // Arrange
            var mockMemory = new Mock<IMemory>();
            mockMemory.Setup(m => m.GetValue(0x200)).Returns(0x12).Verifiable();
            mockMemory.Setup(m => m.GetValue(0x201)).Returns(0xa0).Verifiable();

            var cpu = new Cpu(mockMemory.Object);

            // Act
            cpu.EmulateOp();

            // Assert
            Assert.AreEqual(0x0a2, cpu.PC);
        }

        [Test]
        public void EmulateOp_3XNN_VXEqualToNN_AdvancesProgramCounterBy2()
        {
            var mockMemory = new Mock<IMemory>();
            mockMemory.Setup(m => m.GetValue(0x200)).Returns(0x30).Verifiable();
            mockMemory.Setup(m => m.GetValue(0x201)).Returns(0x12).Verifiable(); // Op Code 3012
            // TODO: Need to set values of CPU register - pull registers into separate object.
        }
    }
}

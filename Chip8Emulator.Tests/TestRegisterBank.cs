using NUnit.Framework;

namespace Chip8Emulator.Core.Core.Tests
{
    [TestFixture]
    public class TestRegisterBank
    {
        [Test]
        public void Constructor_InitialisesRegisters()
        {
            // Arrange
            var registerBank = new RegisterBank();

            // Act
            registerBank.Initialise();
            
            // Assert
            Assert.AreEqual(16, registerBank.V.Length);
        }

        [Test]
        public void Constructor_InitialisesProgramCounter()
        {
            // Arrange
            var registerBank = new RegisterBank();

            // Act
            registerBank.Initialise();

            // Assert
            Assert.AreEqual(0x200, registerBank.PC);
        }

        [Test]
        public void Constructor_InitialisesStackPointer()
        {
            // Arrange
            var registerBank = new RegisterBank();

            // Act
            registerBank.Initialise();

            // Assert
            Assert.AreEqual(0xfa0, registerBank.SP);
        }

        [Test]
        public void Constructor_InitialisesStack()
        {
            // Arrange
            var registerBank = new RegisterBank();

            // Act
            registerBank.Initialise();

            // Assert
            Assert.AreEqual(0, registerBank.Stack.Count);
        }

    }
}

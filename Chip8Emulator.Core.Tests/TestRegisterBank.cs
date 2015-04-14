using NUnit.Framework;

namespace Chip8Emulator.Core.Tests
{
    [TestFixture]
    public class TestRegisterBank
    {
        [Test]
        public void Initialise_InitialisesRegisters()
        {
            // Arrange
            var registerBank = new RegisterBank { V = new byte[] { 0xf, 0xf, 0xf, 0xf, 0xf, 0xf, 0xf, 0xf, 0xf, 0xf, 0xf, 0xf, 0xf, 0xf, 0xf, 0xf } };

            // Act
            registerBank.Initialise();

            // Assert
            Assert.AreEqual(16, registerBank.V.Length);
            Assert.AreEqual(0, registerBank.V[0x00]);
            Assert.AreEqual(0, registerBank.V[0x01]);
            Assert.AreEqual(0, registerBank.V[0x02]);
            Assert.AreEqual(0, registerBank.V[0x03]);
            Assert.AreEqual(0, registerBank.V[0x04]);
            Assert.AreEqual(0, registerBank.V[0x05]);
            Assert.AreEqual(0, registerBank.V[0x06]);
            Assert.AreEqual(0, registerBank.V[0x07]);
            Assert.AreEqual(0, registerBank.V[0x08]);
            Assert.AreEqual(0, registerBank.V[0x09]);
            Assert.AreEqual(0, registerBank.V[0x0a]);
            Assert.AreEqual(0, registerBank.V[0x0b]);
            Assert.AreEqual(0, registerBank.V[0x0c]);
            Assert.AreEqual(0, registerBank.V[0x0d]);
            Assert.AreEqual(0, registerBank.V[0x0e]);
            Assert.AreEqual(0, registerBank.V[0x0f]);
        }

        [Test]
        public void Initialise_InitialisesProgramCounter()
        {
            // Arrange
            var registerBank = new RegisterBank();

            // Act
            registerBank.Initialise();

            // Assert
            Assert.AreEqual(0x200, registerBank.PC);
        }

        [Test]
        public void Initialise_InitialisesStackPointer()
        {
            // Arrange
            var registerBank = new RegisterBank();

            // Act
            registerBank.Initialise();

            // Assert
            Assert.AreEqual(0xfa0, registerBank.SP);
        }

        [Test]
        public void Initialise_InitialisesStack()
        {
            // Arrange
            var registerBank = new RegisterBank();

            // Act
            registerBank.Initialise();

            // Assert
            Assert.AreEqual(0, registerBank.Stack.Count);
        }

        [Test]
        public void Initialise_InitialisesI()
        {
            // Arrange
            var registerBank = new RegisterBank {I = 0xfff};

            // Act
            registerBank.Initialise();

            // Assert
            Assert.AreEqual(0, registerBank.I);
        }

        [Test]
        public void Initialise_InitialisesKey()
        {
            // Arrange
            var registerBank = new RegisterBank { Key = 0x1 };

            // Act
            registerBank.Initialise();

            // Assert
            Assert.AreEqual(0, registerBank.Key);
        }

        [Test]
        public void Initialise_InitialisesKeyPressed()
        {
            // Arrange
            var registerBank = new RegisterBank { KeyPressed = true };

            // Act
            registerBank.Initialise();

            // Assert
            Assert.AreEqual(false, registerBank.KeyPressed);
        }

        [Test]
        public void Initialise_InitialisesDelay()
        {
            // Arrange
            var registerBank = new RegisterBank { Delay = 0xf };

            // Act
            registerBank.Initialise();

            // Assert
            Assert.AreEqual(0, registerBank.Delay);
        }

        [Test]
        public void Initialise_InitialisesKeySound()
        {
            // Arrange
            var registerBank = new RegisterBank { Sound = 0xf };

            // Act
            registerBank.Initialise();

            // Assert
            Assert.AreEqual(0, registerBank.Sound);
        }

    }
}

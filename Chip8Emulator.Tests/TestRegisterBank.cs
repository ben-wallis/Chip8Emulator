using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Chip8Emulator.Tests
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

    }
}

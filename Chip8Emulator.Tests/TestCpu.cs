using Moq;
using NUnit.Framework;

namespace Chip8Emulator.Tests
{
    [TestFixture]
    public class TestCpu
    {
        private CpuTestUtility _testUtility;

        [SetUp]
        public void SetupCpuTestUtility()
        {
            _testUtility = new CpuTestUtility();
        }
        
        [Test]
        public void EmulateOp_1NNN_MovesProgramCounterToAddress()
        {
            // Arrange
            _testUtility.SetOpCodeAtInitialMemoryAddress(0x12a0);

            // Act
            _testUtility.TestCpu.EmulateOp();

            // Assert
            Assert.AreEqual(0x0a2, _testUtility.MockRegisterBank.Object.PC);
        }

        [Test]
        public void EmulateOp_3XNN_VXEqualToNN_AdvancesProgramCounterBy4()
        {
            // Arrange
            const byte TestRegister = 0x01;
            const byte TestValue = 0x12;

            _testUtility.SetOpCodeAtInitialMemoryAddress(0x3112);
            _testUtility.MockRegisterBank.Object.V[TestRegister] = TestValue;

            // Act
            _testUtility.TestCpu.EmulateOp();

            // Assert
            Assert.AreEqual(CpuTestUtility.InitialProgramCounter + 4, _testUtility.MockRegisterBank.Object.PC);
        }

        [Test]
        public void EmulateOp_3XNN_VXNotEqualToNN_AdvancesProgramCounterBy2()
        {
            // Arrange
            const byte TestRegister = 0x01;
            const byte TestValue = 0x01;

            _testUtility.SetOpCodeAtInitialMemoryAddress(0x3112);
            _testUtility.MockRegisterBank.Object.V[TestRegister] = TestValue;

            // Act
            _testUtility.TestCpu.EmulateOp();

            // Assert
            Assert.AreEqual(CpuTestUtility.InitialProgramCounter + 2, _testUtility.MockRegisterBank.Object.PC);
        }

        // Re-usable test utility class to prevent having to manually recreate mocks in every test.
        private class CpuTestUtility
        {
            public const ushort InitialProgramCounter = 0x200;

            public CpuTestUtility()
            {
                // Mock Setups
                MockMemory = new Mock<IMemory>();
                
                MockRegisterBank = new Mock<IRegisterBank>();
                MockRegisterBank.SetupAllProperties();
                MockRegisterBank.Object.V = new byte[16];
                MockRegisterBank.Object.PC = InitialProgramCounter;
                MockRegisterBank.Object.SP = 0xfa0;

                // Class under test instantiation
                TestCpu = new Cpu(MockMemory.Object, MockRegisterBank.Object);
            }
            public Cpu TestCpu { get; private set; }

            public Mock<IMemory> MockMemory { get; private set; }
            public Mock<IRegisterBank> MockRegisterBank { get; private set; }

            public void SetOpCodeAtInitialMemoryAddress(ushort opCode)
            {
                var byte1 = (opCode >> 8) & 0xff;
                var byte2 = opCode & 0x00ff;

                MockMemory.Setup(m => m.GetValue(0x200)).Returns((byte)byte1).Verifiable();
                MockMemory.Setup(m => m.GetValue(0x201)).Returns((byte)byte2).Verifiable();
            }
        }
    }
}

using System.Runtime.CompilerServices;
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
            Assert.AreEqual(0x2a0, _testUtility.MockRegisterBank.Object.PC);
        }

        [Test]
        public void EmulateOp_3XNN_VXEqualToNN_SkipsNextInstruction()
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
        public void EmulateOp_3XNN_VXNotEqualToNN_MovesToNextInstruction()
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

        [Test]
        public void EmulateOp_4XNN_VXNotEqualToNN_SkipsNextInstruction()
        {
            // Arrange
            const byte TestRegister = 0x01;
            const byte TestValue = 0x12;

            _testUtility.SetOpCodeAtInitialMemoryAddress(0x410a);
            _testUtility.MockRegisterBank.Object.V[TestRegister] = TestValue;

            // Act
            _testUtility.TestCpu.EmulateOp();

            // Assert
            Assert.AreEqual(CpuTestUtility.InitialProgramCounter + 4, _testUtility.MockRegisterBank.Object.PC);
        }

        [Test]
        public void EmulateOp_4XNN_VXEqualToNN_MovesToNextInstruction()
        {
            // Arrange
            const byte TestRegister = 0x01;
            const byte TestValue = 0x12;

            _testUtility.SetOpCodeAtInitialMemoryAddress(0x4112);
            _testUtility.MockRegisterBank.Object.V[TestRegister] = TestValue;

            // Act
            _testUtility.TestCpu.EmulateOp();

            // Assert
            Assert.AreEqual(CpuTestUtility.InitialProgramCounter + 2, _testUtility.MockRegisterBank.Object.PC);
        }

        [Test]
        public void EmulateOp_5XY0_VXEqualToVY_SkipsNextInstruction()
        {
            // Arrange
            const byte TestRegisterX = 0x01;
            const byte TestValueX = 0x12;
            const byte TestRegisterY = 0x0b;
            const byte TestValueY = 0x12;

            _testUtility.SetOpCodeAtInitialMemoryAddress(0x51b0);
            _testUtility.MockRegisterBank.Object.V[TestRegisterX] = TestValueX;
            _testUtility.MockRegisterBank.Object.V[TestRegisterY] = TestValueY;

            // Act
            _testUtility.TestCpu.EmulateOp();

            // Assert
            Assert.AreEqual(CpuTestUtility.InitialProgramCounter + 4, _testUtility.MockRegisterBank.Object.PC);
        }

        [Test]
        public void EmulateOp_5XY0_VXNotEqualToVY_SkipsNextInstruction()
        {
            // Arrange
            const byte TestRegisterX = 0x01;
            const byte TestValueX = 0x12;
            const byte TestRegisterY = 0x0b;
            const byte TestValueY = 0x00;

            _testUtility.SetOpCodeAtInitialMemoryAddress(0x51b0);
            _testUtility.MockRegisterBank.Object.V[TestRegisterX] = TestValueX;
            _testUtility.MockRegisterBank.Object.V[TestRegisterY] = TestValueY;

            // Act
            _testUtility.TestCpu.EmulateOp();

            // Assert
            Assert.AreEqual(CpuTestUtility.InitialProgramCounter + 2, _testUtility.MockRegisterBank.Object.PC);
        }

        [Test]
        public void EmulateOp_6XNN_SetsVXToNN()
        {
            // Arrange
            const byte TestRegister = 0x0a;
            const byte TestValue = 0x02;
            
            _testUtility.SetOpCodeAtInitialMemoryAddress(0x6a02);

            // Act
            _testUtility.TestCpu.EmulateOp();

            // Assert
            Assert.AreEqual(_testUtility.MockRegisterBank.Object.V[TestRegister], TestValue);
            Assert.AreEqual(CpuTestUtility.InitialProgramCounter + 2, _testUtility.MockRegisterBank.Object.PC);
        }

        [Test]
        public void EmulateOp_7XNN_AddsNNToVX()
        {
            // Arrange
            const byte TestRegister = 0x0a;
            const byte TestValue = 0x03;
            const byte TestRegisterStartValue = 0xf;

            _testUtility.SetOpCodeAtInitialMemoryAddress(0x7a03);
            _testUtility.MockRegisterBank.Object.V[TestRegister] = TestRegisterStartValue;

            // Act
            _testUtility.TestCpu.EmulateOp();

            // Assert
            Assert.AreEqual(_testUtility.MockRegisterBank.Object.V[TestRegister], TestRegisterStartValue + TestValue);
            Assert.AreEqual(CpuTestUtility.InitialProgramCounter + 2, _testUtility.MockRegisterBank.Object.PC);
        }

        [Test]
        public void EmulateOp_8XY0_SetsVXtoVY()
        {
            // Arrange
            const byte TestRegisterX = 0x01;
            const byte TestValueX = 0x12;
            const byte TestRegisterY = 0x0b;
            const byte TestValueY = 0x00;

            _testUtility.SetOpCodeAtInitialMemoryAddress(0x81b0);
            _testUtility.MockRegisterBank.Object.V[TestRegisterX] = TestValueX;
            _testUtility.MockRegisterBank.Object.V[TestRegisterY] = TestValueY;

            // Act
            _testUtility.TestCpu.EmulateOp();

            // Assert
            Assert.AreEqual(_testUtility.MockRegisterBank.Object.V[TestRegisterY], _testUtility.MockRegisterBank.Object.V[TestRegisterX]);
            Assert.AreEqual(CpuTestUtility.InitialProgramCounter + 2, _testUtility.MockRegisterBank.Object.PC);
        }

        [Test]
        public void EmulateOp_8XY1_SetsVXtoVXORVY()
        {
            // Arrange
            const byte TestRegisterX = 0x01;
            const byte TestValueX = 0xf0;
            const byte TestRegisterY = 0x0b;
            const byte TestValueY = 0x0b;
            const byte ExpectedValueX = 0xfb;

            _testUtility.SetOpCodeAtInitialMemoryAddress(0x81b1);
            _testUtility.MockRegisterBank.Object.V[TestRegisterX] = TestValueX;
            _testUtility.MockRegisterBank.Object.V[TestRegisterY] = TestValueY;

            // Act
            _testUtility.TestCpu.EmulateOp();

            // Assert
            Assert.AreEqual(ExpectedValueX, _testUtility.MockRegisterBank.Object.V[TestRegisterX]);
            Assert.AreEqual(CpuTestUtility.InitialProgramCounter + 2, _testUtility.MockRegisterBank.Object.PC);
        }

        [Test]
        public void EmulateOp_8XY2_SetsVXtoVXANDVY()
        {
            // Arrange
            const byte TestRegisterX = 0x01;
            const byte TestValueX = 0xeb;
            const byte TestRegisterY = 0x0b;
            const byte TestValueY = 0x56;
            const byte ExpectedValueX = 0x42;

            _testUtility.SetOpCodeAtInitialMemoryAddress(0x81b2);
            _testUtility.MockRegisterBank.Object.V[TestRegisterX] = TestValueX;
            _testUtility.MockRegisterBank.Object.V[TestRegisterY] = TestValueY;

            // Act
            _testUtility.TestCpu.EmulateOp();

            // Assert
            Assert.AreEqual(ExpectedValueX, _testUtility.MockRegisterBank.Object.V[TestRegisterX]);
            Assert.AreEqual(CpuTestUtility.InitialProgramCounter + 2, _testUtility.MockRegisterBank.Object.PC);
        }

        [Test]
        public void EmulateOp_8XY3_SetsVXtoVXXORVY()
        {
            // Arrange
            const byte TestRegisterX = 0x01;
            const byte TestValueX = 0x38;
            const byte TestRegisterY = 0x0b;
            const byte TestValueY = 0xb5;
            const byte ExpectedValueX = 0x8d;

            _testUtility.SetOpCodeAtInitialMemoryAddress(0x81b3);
            _testUtility.MockRegisterBank.Object.V[TestRegisterX] = TestValueX;
            _testUtility.MockRegisterBank.Object.V[TestRegisterY] = TestValueY;

            // Act
            _testUtility.TestCpu.EmulateOp();

            // Assert
            Assert.AreEqual(ExpectedValueX, _testUtility.MockRegisterBank.Object.V[TestRegisterX]);
            Assert.AreEqual(CpuTestUtility.InitialProgramCounter + 2, _testUtility.MockRegisterBank.Object.PC);
        }

        [Test]
        public void EmulateOp_9XY0_VXNotEqualToVY_SkipsNextInstruction()
        {
            // Arrange
            const byte TestRegisterX = 0x01;
            const byte TestValueX = 0x12;
            const byte TestRegisterY = 0x0b;
            const byte TestValueY = 0x00;

            _testUtility.SetOpCodeAtInitialMemoryAddress(0x91b0);
            _testUtility.MockRegisterBank.Object.V[TestRegisterX] = TestValueX;
            _testUtility.MockRegisterBank.Object.V[TestRegisterY] = TestValueY;

            // Act
            _testUtility.TestCpu.EmulateOp();

            // Assert
            Assert.AreEqual(CpuTestUtility.InitialProgramCounter + 4, _testUtility.MockRegisterBank.Object.PC);
        }

        [Test]
        public void EmulateOp_9XY0_VXEqualToVY_MovesToNextInstruction()
        {
            // Arrange
            const byte TestRegisterX = 0x01;
            const byte TestValueX = 0x12;
            const byte TestRegisterY = 0x0b;
            const byte TestValueY = 0x12;

            _testUtility.SetOpCodeAtInitialMemoryAddress(0x91b0);
            _testUtility.MockRegisterBank.Object.V[TestRegisterX] = TestValueX;
            _testUtility.MockRegisterBank.Object.V[TestRegisterY] = TestValueY;

            // Act
            _testUtility.TestCpu.EmulateOp();

            // Assert
            Assert.AreEqual(CpuTestUtility.InitialProgramCounter + 2, _testUtility.MockRegisterBank.Object.PC);
        }

        [Test]
        public void EmulateOp_ANNN_SetsIToNNN()
        {
            // Arrange
            const ushort TestAddress = 0xabc;
            _testUtility.SetOpCodeAtInitialMemoryAddress(0xaabc);

            // Act
            _testUtility.TestCpu.EmulateOp();
            
            // Assert
            Assert.AreEqual(TestAddress, _testUtility.MockRegisterBank.Object.I);
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

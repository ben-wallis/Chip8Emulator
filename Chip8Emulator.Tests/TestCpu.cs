using System.Collections.Generic;
using Chip8Emulator.Services;
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
        public void Constructor_CallsRegisterBankInitialise()
        {
            // Arrange
            _testUtility.MockRegisterBank.Setup(r => r.Initialise()).Verifiable();

            // Act

            // Assert
            _testUtility.MockRegisterBank.Verify(r => r.Initialise());
        }

        [Test]
        public void EmulateOp_00E0_CallsDisplayInitialise()
        {
            // Arrange
            _testUtility.SetOpCodeAtInitialMemoryAddress(0x00e0);
            _testUtility.MockDisplay.Setup(d => d.Initialise()).Verifiable();

            // Act
            _testUtility.TestCpu.EmulateOp();

            // Assert
            _testUtility.MockDisplay.Verify(d => d.Initialise());
            Assert.AreEqual(CpuTestUtility.InitialProgramCounter + 2, _testUtility.MockRegisterBank.Object.PC);
        }

        [Test]
        public void EmulateOp_00EE_SetsProgramCounterToValueFromTopOfStack()
        {
            // Arrange
            const ushort StackValue = 0x29a;

            _testUtility.SetOpCodeAtInitialMemoryAddress(0x00ee);
            _testUtility.MockRegisterBank.Object.Stack.Push(StackValue);

            // Act
            _testUtility.TestCpu.EmulateOp();

            // Assert
            Assert.AreEqual(StackValue, _testUtility.MockRegisterBank.Object.PC);
        }

        [Test]
        public void EmulateOp_1NNN_MovesProgramCounterToNNN()
        {
            // Arrange
            const ushort ExpectedResult = 0x2a0;
            _testUtility.SetOpCodeAtInitialMemoryAddress(0x12a0);

            // Act
            _testUtility.TestCpu.EmulateOp();

            // Assert
            Assert.AreEqual(ExpectedResult, _testUtility.MockRegisterBank.Object.PC);
        }

        [Test]
        public void EmulateOp_2NNN_AddsCurrentProgramCounterValueToStack()
        {
            // Arrange
            _testUtility.SetOpCodeAtInitialMemoryAddress(0x22aa);

            // Act
            _testUtility.TestCpu.EmulateOp();

            // Assert
            Assert.AreEqual(CpuTestUtility.InitialProgramCounter, _testUtility.MockRegisterBank.Object.Stack.Pop());
        }


        [Test]
        public void EmulateOp_2NNN_SetsProgramCounterToNNN()
        {
            // Arrange
            const ushort ExpectedResult = 0x2aa;

            _testUtility.SetOpCodeAtInitialMemoryAddress(0x22aa);

            // Act
            _testUtility.TestCpu.EmulateOp();

            // Assert
            Assert.AreEqual(ExpectedResult, _testUtility.MockRegisterBank.Object.PC);
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
        public void EmulateOp_8XY4_NoCarry_SetsVXtoVXPlusVYWithVFUnset()
        {
            // Arrange
            const byte TestRegisterX = 0x01;
            const byte TestValueX = 0x0f;
            const byte TestRegisterY = 0x02;
            const byte TestValueY = 0x0e;
            const byte ExpectedValueX = 0x1d;
            const byte ExpectedValueVf = 0x00;

            _testUtility.SetOpCodeAtInitialMemoryAddress(0x8124);
            _testUtility.MockRegisterBank.Object.V[TestRegisterX] = TestValueX;
            _testUtility.MockRegisterBank.Object.V[TestRegisterY] = TestValueY;

            // Act
            _testUtility.TestCpu.EmulateOp();

            // Assert
            Assert.AreEqual(ExpectedValueX, _testUtility.MockRegisterBank.Object.V[TestRegisterX]);
            Assert.AreEqual(ExpectedValueVf, _testUtility.MockRegisterBank.Object.V[0x0f]);
            Assert.AreEqual(CpuTestUtility.InitialProgramCounter + 2, _testUtility.MockRegisterBank.Object.PC);
        }

        [Test]
        public void EmulateOp_8XY4_Carry_SetsVXtoRemainderOfVXPlusVYWithVFSet()
        {
            // Arrange
            const byte TestRegisterX = 0x01;
            const byte TestValueX = 0xf0;
            const byte TestRegisterY = 0x02;
            const byte TestValueY = 0x40;
            const byte ExpectedValueX = 0x31;
            const byte ExpectedValueVf = 0x01;

            _testUtility.SetOpCodeAtInitialMemoryAddress(0x8124);
            _testUtility.MockRegisterBank.Object.V[TestRegisterX] = TestValueX;
            _testUtility.MockRegisterBank.Object.V[TestRegisterY] = TestValueY;

            // Act
            _testUtility.TestCpu.EmulateOp();

            // Assert
            Assert.AreEqual(ExpectedValueX, _testUtility.MockRegisterBank.Object.V[TestRegisterX]);
            Assert.AreEqual(ExpectedValueVf, _testUtility.MockRegisterBank.Object.V[0x0f]);
            Assert.AreEqual(CpuTestUtility.InitialProgramCounter + 2, _testUtility.MockRegisterBank.Object.PC);
        }

        [Test]
        public void EmulateOp_8XY5_NoBorrow_SetsVXtoVXMinusVYWithVFSet()
        {
            // Arrange
            const byte TestRegisterX = 0x01;
            const byte TestValueX = 0x1f;
            const byte TestRegisterY = 0x02;
            const byte TestValueY = 0x01;
            const byte ExpectedValueX = 0x1e;
            const byte ExpectedValueVf = 0x01;

            _testUtility.SetOpCodeAtInitialMemoryAddress(0x8125);
            _testUtility.MockRegisterBank.Object.V[TestRegisterX] = TestValueX;
            _testUtility.MockRegisterBank.Object.V[TestRegisterY] = TestValueY;

            // Act
            _testUtility.TestCpu.EmulateOp();

            // Assert
            Assert.AreEqual(ExpectedValueX, _testUtility.MockRegisterBank.Object.V[TestRegisterX]);
            Assert.AreEqual(ExpectedValueVf, _testUtility.MockRegisterBank.Object.V[0x0f]);
            Assert.AreEqual(CpuTestUtility.InitialProgramCounter + 2, _testUtility.MockRegisterBank.Object.PC);
        }

        [Test]
        public void EmulateOp_8XY5_Borrow_SetsVXtoVXMinusVYWithVFUnset()
        {
            // Arrange
            const byte TestRegisterX = 0x01;
            const byte TestValueX = 0x01;
            const byte TestRegisterY = 0x02;
            const byte TestValueY = 0x1f;
            const byte ExpectedValueX = 0xe2;
            const byte ExpectedValueVf = 0x00;

            _testUtility.SetOpCodeAtInitialMemoryAddress(0x8125);
            _testUtility.MockRegisterBank.Object.V[TestRegisterX] = TestValueX;
            _testUtility.MockRegisterBank.Object.V[TestRegisterY] = TestValueY;

            // Act
            _testUtility.TestCpu.EmulateOp();

            // Assert
            Assert.AreEqual(ExpectedValueX, _testUtility.MockRegisterBank.Object.V[TestRegisterX]);
            Assert.AreEqual(ExpectedValueVf, _testUtility.MockRegisterBank.Object.V[0x0f]);
            Assert.AreEqual(CpuTestUtility.InitialProgramCounter + 2, _testUtility.MockRegisterBank.Object.PC);
        }

        [Test]
        public void EmulateOp_8XY6_ShiftsVXRightByOne()
        {
            // Arrange
            const byte TestRegisterX = 0x01;
            const byte TestValueX = 0xef;
            const byte ExpectedValueX = 0x0e;

            _testUtility.SetOpCodeAtInitialMemoryAddress(0x8106);
            _testUtility.MockRegisterBank.Object.V[TestRegisterX] = TestValueX;

            // Act
            _testUtility.TestCpu.EmulateOp();

            // Assert
            Assert.AreEqual(ExpectedValueX, _testUtility.MockRegisterBank.Object.V[TestRegisterX]);
            Assert.AreEqual(CpuTestUtility.InitialProgramCounter + 2, _testUtility.MockRegisterBank.Object.PC);
        }

        [Test]
        public void EmulateOp_8XY6_SetsVfToLeastSignificantBit()
        {
            // Arrange
            const byte TestRegisterX = 0x01;
            const byte TestValueX = 0xef;
            const byte ExpectedValueVf = 0x0f;

            _testUtility.SetOpCodeAtInitialMemoryAddress(0x8106);
            _testUtility.MockRegisterBank.Object.V[TestRegisterX] = TestValueX;

            // Act
            _testUtility.TestCpu.EmulateOp();

            // Assert
            Assert.AreEqual(ExpectedValueVf, _testUtility.MockRegisterBank.Object.V[0x0f]);
            Assert.AreEqual(CpuTestUtility.InitialProgramCounter + 2, _testUtility.MockRegisterBank.Object.PC);
        }

        [Test]
        public void EmulateOp_8XY7_NoBorrow_SetsVXtoVYMinusVXWithVFSet()
        {
            // Arrange
            const byte TestRegisterX = 0x01;
            const byte TestValueX = 0x01;
            const byte TestRegisterY = 0x02;
            const byte TestValueY = 0x1f;
            const byte ExpectedValueX = 0x1e;
            const byte ExpectedValueVf = 0x01;

            _testUtility.SetOpCodeAtInitialMemoryAddress(0x8127);
            _testUtility.MockRegisterBank.Object.V[TestRegisterX] = TestValueX;
            _testUtility.MockRegisterBank.Object.V[TestRegisterY] = TestValueY;

            // Act
            _testUtility.TestCpu.EmulateOp();

            // Assert
            Assert.AreEqual(ExpectedValueX, _testUtility.MockRegisterBank.Object.V[TestRegisterX]);
            Assert.AreEqual(ExpectedValueVf, _testUtility.MockRegisterBank.Object.V[0x0f]);
            Assert.AreEqual(CpuTestUtility.InitialProgramCounter + 2, _testUtility.MockRegisterBank.Object.PC);
        }

        [Test]
        public void EmulateOp_8XY7_Borrow_SetsVXtoVYMinusVXWithVFUnset()
        {
            // Arrange
            const byte TestRegisterX = 0x01;
            const byte TestValueX = 0x1f;
            const byte TestRegisterY = 0x02;
            const byte TestValueY = 0x01;
            const byte ExpectedValueX = 0xe2;
            const byte ExpectedValueVf = 0x00;

            _testUtility.SetOpCodeAtInitialMemoryAddress(0x8127);
            _testUtility.MockRegisterBank.Object.V[TestRegisterX] = TestValueX;
            _testUtility.MockRegisterBank.Object.V[TestRegisterY] = TestValueY;

            // Act
            _testUtility.TestCpu.EmulateOp();

            // Assert
            Assert.AreEqual(ExpectedValueX, _testUtility.MockRegisterBank.Object.V[TestRegisterX]);
            Assert.AreEqual(ExpectedValueVf, _testUtility.MockRegisterBank.Object.V[0x0f]);
            Assert.AreEqual(CpuTestUtility.InitialProgramCounter + 2, _testUtility.MockRegisterBank.Object.PC);
        }

        [Test]
        public void EmulateOp_8XYE_ShiftsVXLeftByOne()
        {
            // Arrange
            const byte TestRegisterX = 0x01;
            const byte TestValueX = 0xef;
            const byte ExpectedValueX = 0xf0;

            _testUtility.SetOpCodeAtInitialMemoryAddress(0x810e);
            _testUtility.MockRegisterBank.Object.V[TestRegisterX] = TestValueX;

            // Act
            _testUtility.TestCpu.EmulateOp();

            // Assert
            Assert.AreEqual(ExpectedValueX, _testUtility.MockRegisterBank.Object.V[TestRegisterX]);
            Assert.AreEqual(CpuTestUtility.InitialProgramCounter + 2, _testUtility.MockRegisterBank.Object.PC);
        }

        [Test]
        public void EmulateOp_8XYE_SetsVfToMostSignificantBit()
        {
            // Arrange
            const byte TestRegisterX = 0x01;
            const byte TestValueX = 0xef;
            const byte ExpectedValueVf = 0x0e;

            _testUtility.SetOpCodeAtInitialMemoryAddress(0x810e);
            _testUtility.MockRegisterBank.Object.V[TestRegisterX] = TestValueX;

            // Act
            _testUtility.TestCpu.EmulateOp();

            // Assert
            Assert.AreEqual(ExpectedValueVf, _testUtility.MockRegisterBank.Object.V[0x0f]);
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

        [Test]
        public void EmulateOp_BNNN_MovesProgramCounterToNNNPlusV0()
        {
            // Arrange
            const ushort TestAddress = 0x222;
            const byte TestV0Value = 0xf;
            const ushort ExpectedResult = TestAddress + TestV0Value;

            _testUtility.SetOpCodeAtInitialMemoryAddress(0xb222);
            _testUtility.MockRegisterBank.Object.V[0] = TestV0Value;

            // Act
            _testUtility.TestCpu.EmulateOp();

            // Assert
            Assert.AreEqual(ExpectedResult, _testUtility.MockRegisterBank.Object.PC);
        }

        [Test]
        public void EmulateOp_CXNN_SetsVXToValueReturnedByRandomServiceMaskedByNN()
        {
            // Arrange
            const byte TestRegisterX = 0x01;
            const byte ReturnedRandomByte = 0xef;
            const byte ExpectedResult = 0xab;

            _testUtility.SetOpCodeAtInitialMemoryAddress(0xc1bb);
            _testUtility.MockRandomService.Setup(r => r.GetRandomByte()).Returns(ReturnedRandomByte).Verifiable();

            // Act
            _testUtility.TestCpu.EmulateOp();

            // Assert
            Assert.AreEqual(ExpectedResult, _testUtility.MockRegisterBank.Object.V[TestRegisterX]);
            _testUtility.MockRandomService.Verify(r => r.GetRandomByte());
        }

        [Test]
        public void EmulateOp_DXYN_SetsDrawRequiredTrue()
        {
            // Arrange
            const byte TestRegisterX = 0x01;
            const byte TestRegisterXValue = 0x01;
            const byte TestRegisterY = 0x02;
            const byte TestRegisterYValue = 0x02;
            const byte TestIValue = 0x00;

            _testUtility.SetOpCodeAtInitialMemoryAddress(0xd121);
            _testUtility.MockRegisterBank.Object.I = TestIValue;
            _testUtility.MockRegisterBank.Object.V[TestRegisterX] = TestRegisterXValue;
            _testUtility.MockRegisterBank.Object.V[TestRegisterY] = TestRegisterYValue;

            // Act
            _testUtility.TestCpu.EmulateOp();

            // Assert
            Assert.AreEqual(true, _testUtility.TestCpu.DrawRequired);
            Assert.AreEqual(CpuTestUtility.InitialProgramCounter + 2, _testUtility.MockRegisterBank.Object.PC);
        }

        [Test]
        public void EmulateOp_DXYN_NoWrap_NoSetPixelsUnset_SetsCorrectDisplayPixels()
        {
            // Arrange
            const byte TestRegisterX = 0x01;
            const byte TestRegisterXValue = 0x01;
            const byte TestRegisterY = 0x02;
            const byte TestRegisterYValue = 0x02;
            const byte TestIValue = 0x00;

            _testUtility.SetOpCodeAtInitialMemoryAddress(0xd125);
            _testUtility.MockRegisterBank.Object.I = TestIValue;
            _testUtility.MockRegisterBank.Object.V[TestRegisterX] = TestRegisterXValue;
            _testUtility.MockRegisterBank.Object.V[TestRegisterY] = TestRegisterYValue;

            // Set the Memory mock to return the data for a "0" character
            _testUtility.MockMemory.Setup(m => m.GetValue(0x00)).Returns(0xf0).Verifiable(); // b11110000
            _testUtility.MockMemory.Setup(m => m.GetValue(0x01)).Returns(0x90).Verifiable(); // b10010000
            _testUtility.MockMemory.Setup(m => m.GetValue(0x02)).Returns(0x90).Verifiable(); // b10010000
            _testUtility.MockMemory.Setup(m => m.GetValue(0x03)).Returns(0x90).Verifiable(); // b10010000
            _testUtility.MockMemory.Setup(m => m.GetValue(0x04)).Returns(0xf0).Verifiable(); // b11110000

            _testUtility.MockDisplay.Setup(d => d.FlipPixel(0x01, 0x02)).Returns(false).Verifiable();
            _testUtility.MockDisplay.Setup(d => d.FlipPixel(0x02, 0x02)).Returns(false).Verifiable();
            _testUtility.MockDisplay.Setup(d => d.FlipPixel(0x03, 0x02)).Returns(false).Verifiable();
            _testUtility.MockDisplay.Setup(d => d.FlipPixel(0x04, 0x02)).Returns(false).Verifiable();
            _testUtility.MockDisplay.Setup(d => d.FlipPixel(0x01, 0x03)).Returns(false).Verifiable();
            _testUtility.MockDisplay.Setup(d => d.FlipPixel(0x04, 0x03)).Returns(false).Verifiable();
            _testUtility.MockDisplay.Setup(d => d.FlipPixel(0x01, 0x04)).Returns(false).Verifiable();
            _testUtility.MockDisplay.Setup(d => d.FlipPixel(0x04, 0x04)).Returns(false).Verifiable();
            _testUtility.MockDisplay.Setup(d => d.FlipPixel(0x01, 0x05)).Returns(false).Verifiable();
            _testUtility.MockDisplay.Setup(d => d.FlipPixel(0x04, 0x05)).Returns(false).Verifiable();
            _testUtility.MockDisplay.Setup(d => d.FlipPixel(0x01, 0x06)).Returns(false).Verifiable();
            _testUtility.MockDisplay.Setup(d => d.FlipPixel(0x02, 0x06)).Returns(false).Verifiable();
            _testUtility.MockDisplay.Setup(d => d.FlipPixel(0x03, 0x06)).Returns(false).Verifiable();
            _testUtility.MockDisplay.Setup(d => d.FlipPixel(0x04, 0x06)).Returns(false).Verifiable();

            // Act
            _testUtility.TestCpu.EmulateOp();

            // Assert
            _testUtility.MockMemory.VerifyAll();
            _testUtility.MockDisplay.VerifyAll();
            
            Assert.AreEqual(0x00, _testUtility.MockRegisterBank.Object.V[0x0f]);
            Assert.AreEqual(CpuTestUtility.InitialProgramCounter + 2, _testUtility.MockRegisterBank.Object.PC);
        }

        [Test]
        public void EmulateOp_DXYN_NoWrap_SetPixelsUnset_SetsVFTo1()
        {
            // Arrange
            const byte TestRegisterX = 0x01;
            const byte TestRegisterXValue = 0x01;
            const byte TestRegisterY = 0x02;
            const byte TestRegisterYValue = 0x02;
            const byte TestIValue = 0x00;

            _testUtility.SetOpCodeAtInitialMemoryAddress(0xd121);
            _testUtility.MockRegisterBank.Object.I = TestIValue;
            _testUtility.MockRegisterBank.Object.V[TestRegisterX] = TestRegisterXValue;
            _testUtility.MockRegisterBank.Object.V[TestRegisterY] = TestRegisterYValue;

            // Set the Memory mock to return the data for the first line of a "0" character
            _testUtility.MockMemory.Setup(m => m.GetValue(0x00)).Returns(0xf0).Verifiable(); // b11110000

            _testUtility.MockDisplay.Setup(d => d.FlipPixel(0x01, 0x02)).Returns(false).Verifiable();
            _testUtility.MockDisplay.Setup(d => d.FlipPixel(0x02, 0x02)).Returns(true).Verifiable();
            _testUtility.MockDisplay.Setup(d => d.FlipPixel(0x03, 0x02)).Returns(false).Verifiable();
            _testUtility.MockDisplay.Setup(d => d.FlipPixel(0x04, 0x02)).Returns(false).Verifiable();

            // Act
            _testUtility.TestCpu.EmulateOp();

            // Assert
            _testUtility.MockMemory.VerifyAll();
            _testUtility.MockDisplay.VerifyAll();
            Assert.AreEqual(0x01, _testUtility.MockRegisterBank.Object.V[0x0f]);
            Assert.AreEqual(CpuTestUtility.InitialProgramCounter + 2, _testUtility.MockRegisterBank.Object.PC);
        }

        [Test]
        public void EmulateOp_DXYN_WrapX_NoSetPixelsUnset_DoesntCallFlipPixelForOverflowedPixels()
        {
            // Arrange
            const byte TestRegisterX = 0x01;
            const byte TestRegisterXValue = 0x3c; // 60 (4 pixels will overflow the right side of the 64x32 display)
            const byte TestRegisterY = 0x02;
            const byte TestRegisterYValue = 0x02;
            const byte TestIValue = 0x00;

            _testUtility.SetOpCodeAtInitialMemoryAddress(0xd121);
            _testUtility.MockRegisterBank.Object.I = TestIValue;
            _testUtility.MockRegisterBank.Object.V[TestRegisterX] = TestRegisterXValue;
            _testUtility.MockRegisterBank.Object.V[TestRegisterY] = TestRegisterYValue;

            // Set the Memory mock to return a full line of 1s
            _testUtility.MockMemory.Setup(m => m.GetValue(0x00)).Returns(0xff).Verifiable(); // b11111111

            _testUtility.MockDisplay.Setup(d => d.FlipPixel(0x3c, 0x02)).Returns(false).Verifiable();
            _testUtility.MockDisplay.Setup(d => d.FlipPixel(0x3d, 0x02)).Returns(false).Verifiable();
            _testUtility.MockDisplay.Setup(d => d.FlipPixel(0x3e, 0x02)).Returns(false).Verifiable();
            _testUtility.MockDisplay.Setup(d => d.FlipPixel(0x3f, 0x02)).Returns(false).Verifiable();

            // Act
            _testUtility.TestCpu.EmulateOp();

            // Assert
            _testUtility.MockMemory.VerifyAll();
            _testUtility.MockDisplay.Verify(d => d.FlipPixel(0x40, 0x02), Times.Never);
            _testUtility.MockDisplay.Verify(d => d.FlipPixel(0x41, 0x02), Times.Never);
            _testUtility.MockDisplay.Verify(d => d.FlipPixel(0x42, 0x02), Times.Never);
            _testUtility.MockDisplay.Verify(d => d.FlipPixel(0x43, 0x02), Times.Never);
            Assert.AreEqual(CpuTestUtility.InitialProgramCounter + 2, _testUtility.MockRegisterBank.Object.PC);
        }

        [Test]
        public void EmulateOp_DXYN_WrapY_NoSetPixelsUnset_DoesntCallFlipPixelForOverflowedPixels()
        {
            // Arrange
            const byte TestRegisterX = 0x01;
            const byte TestRegisterXValue = 0x01;
            const byte TestRegisterY = 0x02;
            const byte TestRegisterYValue = 0x1f; // 31 (the second row will overflow the bottom of the 64x32 display)
            const byte TestIValue = 0x00;

            _testUtility.SetOpCodeAtInitialMemoryAddress(0xd122);
            _testUtility.MockRegisterBank.Object.I = TestIValue;
            _testUtility.MockRegisterBank.Object.V[TestRegisterX] = TestRegisterXValue;
            _testUtility.MockRegisterBank.Object.V[TestRegisterY] = TestRegisterYValue;

            _testUtility.MockMemory.Setup(m => m.GetValue(0x00)).Returns(0xf0).Verifiable(); // b11110000

            _testUtility.MockDisplay.Setup(d => d.FlipPixel(0x01, 0x1f)).Returns(false).Verifiable();
            _testUtility.MockDisplay.Setup(d => d.FlipPixel(0x02, 0x1f)).Returns(false).Verifiable();
            _testUtility.MockDisplay.Setup(d => d.FlipPixel(0x03, 0x1f)).Returns(false).Verifiable();
            _testUtility.MockDisplay.Setup(d => d.FlipPixel(0x04, 0x1f)).Returns(false).Verifiable();

            // Act
            _testUtility.TestCpu.EmulateOp();

            // Assert
            _testUtility.MockMemory.VerifyAll();
            _testUtility.MockDisplay.Verify(d => d.FlipPixel(0x01, 0x20), Times.Never);
            _testUtility.MockDisplay.Verify(d => d.FlipPixel(0x02, 0x20), Times.Never);
            _testUtility.MockDisplay.Verify(d => d.FlipPixel(0x03, 0x20), Times.Never);
            _testUtility.MockDisplay.Verify(d => d.FlipPixel(0x04, 0x20), Times.Never);
            Assert.AreEqual(CpuTestUtility.InitialProgramCounter + 2, _testUtility.MockRegisterBank.Object.PC);
        }

        [Test]
        public void EmulateOp_EX9E_KeyStoredInVXPressed_SkipsNextInstruction()
        {
            // Arrange
            const byte TestRegisterX = 0x01;
            const byte TestKeyX = 0x05;

            _testUtility.SetOpCodeAtInitialMemoryAddress(0xe19e);
            _testUtility.MockRegisterBank.Object.V[TestRegisterX] = TestKeyX;
            _testUtility.MockRegisterBank.Object.KeyPressed = true;
            _testUtility.MockRegisterBank.Object.Key = TestKeyX;

            // Act
            _testUtility.TestCpu.EmulateOp();

            // Assert
            Assert.AreEqual(CpuTestUtility.InitialProgramCounter + 4, _testUtility.MockRegisterBank.Object.PC);
        }

        [Test]
        public void EmulateOp_EX9E_NoKeyPressed_MovesToNextInstruction()
        {
            // Arrange
            const byte TestRegisterX = 0x01;
            const byte TestKeyX = 0x05;

            _testUtility.SetOpCodeAtInitialMemoryAddress(0xe19e);
            _testUtility.MockRegisterBank.Object.V[TestRegisterX] = TestKeyX;
            _testUtility.MockRegisterBank.Object.KeyPressed = false;

            // Act
            _testUtility.TestCpu.EmulateOp();

            // Assert
            Assert.AreEqual(CpuTestUtility.InitialProgramCounter + 2, _testUtility.MockRegisterBank.Object.PC);
        }

        [Test]
        public void EmulateOp_EX9E_DifferentKeyPressed_MovesToNextInstruction()
        {
            // Arrange
            const byte TestRegisterX = 0x01;
            const byte TestKeyX = 0x05;

            _testUtility.SetOpCodeAtInitialMemoryAddress(0xe19e);
            _testUtility.MockRegisterBank.Object.V[TestRegisterX] = TestKeyX;
            _testUtility.MockRegisterBank.Object.Key = 0x0a;
            _testUtility.MockRegisterBank.Object.KeyPressed = true;

            // Act
            _testUtility.TestCpu.EmulateOp();

            // Assert
            Assert.AreEqual(CpuTestUtility.InitialProgramCounter + 2, _testUtility.MockRegisterBank.Object.PC);
        }

        [Test]
        public void EmulateOp_EX1A_KeyStoredInVXPressed_MovesToNextInstruction()
        {
            // Arrange
            const byte TestRegisterX = 0x01;
            const byte TestKeyX = 0x05;

            _testUtility.SetOpCodeAtInitialMemoryAddress(0xe1a1);
            _testUtility.MockRegisterBank.Object.V[TestRegisterX] = TestKeyX;
            _testUtility.MockRegisterBank.Object.Key = TestKeyX;
            _testUtility.MockRegisterBank.Object.KeyPressed = true;

            // Act
            _testUtility.TestCpu.EmulateOp();

            // Assert
            Assert.AreEqual(CpuTestUtility.InitialProgramCounter + 2, _testUtility.MockRegisterBank.Object.PC);
        }

        [Test]
        public void EmulateOp_EX1A_NoKeyPressed_SkipsNextInstruction()
        {
            // Arrange
            const byte TestRegisterX = 0x01;
            const byte TestKeyX = 0x05;

            _testUtility.SetOpCodeAtInitialMemoryAddress(0xe1a1);
            _testUtility.MockRegisterBank.Object.V[TestRegisterX] = TestKeyX;
            _testUtility.MockRegisterBank.Object.KeyPressed = false;

            // Act
            _testUtility.TestCpu.EmulateOp();

            // Assert
            Assert.AreEqual(CpuTestUtility.InitialProgramCounter + 4, _testUtility.MockRegisterBank.Object.PC);
        }

        [Test]
        public void EmulateOp_EX1A_DifferentKeyPressed_SkipsNextInstruction()
        {
            // Arrange
            const byte TestRegisterX = 0x01;
            const byte TestKeyX = 0x05;

            _testUtility.SetOpCodeAtInitialMemoryAddress(0xe1a1);
            _testUtility.MockRegisterBank.Object.V[TestRegisterX] = TestKeyX;
            _testUtility.MockRegisterBank.Object.Key = 0x0a;
            _testUtility.MockRegisterBank.Object.KeyPressed = true;

            // Act
            _testUtility.TestCpu.EmulateOp();

            // Assert
            Assert.AreEqual(CpuTestUtility.InitialProgramCounter + 4, _testUtility.MockRegisterBank.Object.PC);
        }

        [Test]
        public void EmulateOp_FX07_SetsVXToDelayTimerValue()
        {
            // Arrange
            const byte TestRegisterX = 0x01;
            const byte TestDelay = 0x1b;

            _testUtility.SetOpCodeAtInitialMemoryAddress(0xf107);
            _testUtility.MockRegisterBank.Object.Delay = TestDelay;

            // Act
            _testUtility.TestCpu.EmulateOp();

            // Assert
            Assert.AreEqual(TestDelay, _testUtility.MockRegisterBank.Object.V[TestRegisterX]);
            Assert.AreEqual(CpuTestUtility.InitialProgramCounter + 2, _testUtility.MockRegisterBank.Object.PC);
        }

        [Test]
        public void EmulateOp_FX0A_NoKeyPressed_DoesNotAdvanceProgramCounter()
        {
            // Arrange
            _testUtility.SetOpCodeAtInitialMemoryAddress(0xf10a);
            _testUtility.MockRegisterBank.Object.KeyPressed = false;

            // Act
            _testUtility.TestCpu.EmulateOp();

            // Assert
            Assert.AreEqual(CpuTestUtility.InitialProgramCounter, _testUtility.MockRegisterBank.Object.PC);
        }

        [Test]
        public void EmulateOp_FX0A_KeyPressed_StoresKeyInVX()
        {
            // Arrange
            const byte TestRegisterX = 0x0a;
            const byte TestKey = 0x04;

            _testUtility.SetOpCodeAtInitialMemoryAddress(0xfa0a);
            _testUtility.MockRegisterBank.Object.KeyPressed = true;
            _testUtility.MockRegisterBank.Object.Key = TestKey;

            // Act
            _testUtility.TestCpu.EmulateOp();

            // Assert
            Assert.AreEqual(TestKey, _testUtility.MockRegisterBank.Object.V[TestRegisterX]);
            Assert.AreEqual(CpuTestUtility.InitialProgramCounter + 2, _testUtility.MockRegisterBank.Object.PC);
        }

        [Test]
        public void EmulateOp_FX15_SetsDelayTimerToVX()
        {
            // Arrange
            const byte TestRegisterX = 0x01;
            const byte TestRegisterXValue = 0x1b;

            _testUtility.SetOpCodeAtInitialMemoryAddress(0xf115);
            _testUtility.MockRegisterBank.Object.V[TestRegisterX] = TestRegisterXValue;

            // Act
            _testUtility.TestCpu.EmulateOp();

            // Assert
            Assert.AreEqual(TestRegisterXValue, _testUtility.MockRegisterBank.Object.Delay);
            Assert.AreEqual(CpuTestUtility.InitialProgramCounter + 2, _testUtility.MockRegisterBank.Object.PC);
        }

        [Test]
        public void EmulateOp_FX18_SetsSoundTimerToVX()
        {
            // Arrange
            const byte TestRegisterX = 0x01;
            const byte TestRegisterXValue = 0x1b;

            _testUtility.SetOpCodeAtInitialMemoryAddress(0xf118);
            _testUtility.MockRegisterBank.Object.V[TestRegisterX] = TestRegisterXValue;

            // Act
            _testUtility.TestCpu.EmulateOp();

            // Assert
            Assert.AreEqual(TestRegisterXValue, _testUtility.MockRegisterBank.Object.Sound);
            Assert.AreEqual(CpuTestUtility.InitialProgramCounter + 2, _testUtility.MockRegisterBank.Object.PC);
        }

        [Test]
        public void EmulateOp_FX1E_AddsVXToI()
        {
            // Arrange
            const byte TestRegisterX = 0x01;
            const byte TestRegisterXValue = 0x1b;
            const ushort TestIValue = 0xff2;
            const ushort ExpectedValue = TestIValue + TestRegisterXValue;

            _testUtility.SetOpCodeAtInitialMemoryAddress(0xf11e);
            _testUtility.MockRegisterBank.Object.V[TestRegisterX] = TestRegisterXValue;
            _testUtility.MockRegisterBank.Object.I = TestIValue;

            // Act
            _testUtility.TestCpu.EmulateOp();

            // Assert
            Assert.AreEqual(ExpectedValue, _testUtility.MockRegisterBank.Object.I);
            Assert.AreEqual(CpuTestUtility.InitialProgramCounter + 2, _testUtility.MockRegisterBank.Object.PC);
        }

        [Test]
        public void EmulateOp_FX29_SetsIToLocationOfSpriteForDigitX()
        {
            // Arrange
            const byte TestRegisterX = 0x01;
            const byte TestRegisterXValue = 0x02;
            const ushort ExpectedAddress = 0x0a;

            _testUtility.SetOpCodeAtInitialMemoryAddress(0xf129);
            _testUtility.MockRegisterBank.Object.V[TestRegisterX] = TestRegisterXValue;

            // Act
            _testUtility.TestCpu.EmulateOp();

            // Assert
            Assert.AreEqual(ExpectedAddress, _testUtility.MockRegisterBank.Object.I);
            Assert.AreEqual(CpuTestUtility.InitialProgramCounter + 2, _testUtility.MockRegisterBank.Object.PC);
        }

        [Test]
        public void EmulateOp_FX33_StoresVXConvertedToBCDInMemoryAtAddressI()
        {
            // Arrange
            const byte TestRegisterX = 0x01;
            const byte TestRegisterXValue = 0xad; // 173
            const ushort TestIValue = 0x268;
            const byte ExpectedHundreds = 0x01;
            const byte ExpectedTens = 0x07;
            const byte ExpectedOnes = 0x03;
            
            _testUtility.SetOpCodeAtInitialMemoryAddress(0xf133);
            _testUtility.MockRegisterBank.Object.V[TestRegisterX] = TestRegisterXValue;
            _testUtility.MockRegisterBank.Object.I = TestIValue;

            _testUtility.MockMemory.Setup(m => m.SetValue(TestIValue, ExpectedHundreds)).Verifiable();
            _testUtility.MockMemory.Setup(m => m.SetValue(TestIValue + 1, ExpectedTens)).Verifiable();
            _testUtility.MockMemory.Setup(m => m.SetValue(TestIValue + 2, ExpectedOnes)).Verifiable();

            // Act
            _testUtility.TestCpu.EmulateOp();

            // Assert
            _testUtility.MockMemory.VerifyAll();
            Assert.AreEqual(CpuTestUtility.InitialProgramCounter + 2, _testUtility.MockRegisterBank.Object.PC);
        }

        [Test]
        public void EmulateOp_FX55_StoresV0toVXInMemoryAtAddressI()
        {
            // Arrange
            const ushort TestIValue = 0xc00;

            const byte TestRegister0Value = 0x01;
            const byte TestRegister1Value = 0xbc;
            const byte TestRegister2Value = 0xa5;
            const byte TestRegister3Value = 0xe2;
            const byte TestRegister4Value = 0x00;
            const byte TestRegister5Value = 0x10;
            const byte TestRegister6Value = 0xaf;
            const byte TestRegister7Value = 0xff;
            const byte TestRegister8Value = 0xf9;
            const byte TestRegister9Value = 0x8e;
            const byte TestRegisterAValue = 0x11;

            _testUtility.SetOpCodeAtInitialMemoryAddress(0xfa55);

            _testUtility.MockRegisterBank.Object.I = TestIValue;
            _testUtility.MockRegisterBank.Object.V[0x00] = TestRegister0Value;
            _testUtility.MockRegisterBank.Object.V[0x01] = TestRegister1Value;
            _testUtility.MockRegisterBank.Object.V[0x02] = TestRegister2Value;
            _testUtility.MockRegisterBank.Object.V[0x03] = TestRegister3Value;
            _testUtility.MockRegisterBank.Object.V[0x04] = TestRegister4Value;
            _testUtility.MockRegisterBank.Object.V[0x05] = TestRegister5Value;
            _testUtility.MockRegisterBank.Object.V[0x06] = TestRegister6Value;
            _testUtility.MockRegisterBank.Object.V[0x07] = TestRegister7Value;
            _testUtility.MockRegisterBank.Object.V[0x08] = TestRegister8Value;
            _testUtility.MockRegisterBank.Object.V[0x09] = TestRegister9Value;
            _testUtility.MockRegisterBank.Object.V[0xa] = TestRegisterAValue;

            _testUtility.MockMemory.Setup(m => m.SetValue(TestIValue + 0, TestRegister0Value)).Verifiable();
            _testUtility.MockMemory.Setup(m => m.SetValue(TestIValue + 1, TestRegister1Value)).Verifiable();
            _testUtility.MockMemory.Setup(m => m.SetValue(TestIValue + 2, TestRegister2Value)).Verifiable();
            _testUtility.MockMemory.Setup(m => m.SetValue(TestIValue + 3, TestRegister3Value)).Verifiable();
            _testUtility.MockMemory.Setup(m => m.SetValue(TestIValue + 4, TestRegister4Value)).Verifiable();
            _testUtility.MockMemory.Setup(m => m.SetValue(TestIValue + 5, TestRegister5Value)).Verifiable();
            _testUtility.MockMemory.Setup(m => m.SetValue(TestIValue + 6, TestRegister6Value)).Verifiable();
            _testUtility.MockMemory.Setup(m => m.SetValue(TestIValue + 7, TestRegister7Value)).Verifiable();
            _testUtility.MockMemory.Setup(m => m.SetValue(TestIValue + 8, TestRegister8Value)).Verifiable();
            _testUtility.MockMemory.Setup(m => m.SetValue(TestIValue + 9, TestRegister9Value)).Verifiable();
            _testUtility.MockMemory.Setup(m => m.SetValue(TestIValue + 10, TestRegisterAValue)).Verifiable();

            // Act
            _testUtility.TestCpu.EmulateOp();

            // Assert
            _testUtility.MockMemory.VerifyAll();
            Assert.AreEqual(CpuTestUtility.InitialProgramCounter + 2, _testUtility.MockRegisterBank.Object.PC);
        }

        [Test]
        public void EmulateOp_FX65_FillV0ToVXFromMemoryAtAddressI()
        {
            // Arrange
            const ushort TestIValue = 0xc00;

            const byte ExpectedRegister0Value = 0x01;
            const byte ExpectedRegister1Value = 0xbc;
            const byte ExpectedRegister2Value = 0xa5;
            const byte ExpectedRegister3Value = 0xe2;
            const byte ExpectedRegister4Value = 0x00;
            const byte ExpectedRegister5Value = 0x10;
            const byte ExpectedRegister6Value = 0xaf;
            const byte ExpectedRegister7Value = 0xff;
            const byte ExpectedRegister8Value = 0xf9;
            const byte ExpectedRegister9Value = 0x8e;
            const byte ExpectedRegisterAValue = 0x11;

            _testUtility.SetOpCodeAtInitialMemoryAddress(0xfa65);

            _testUtility.MockRegisterBank.Object.I = TestIValue;

            _testUtility.MockMemory.Setup(m => m.GetValue(TestIValue + 0)).Returns(ExpectedRegister0Value).Verifiable();
            _testUtility.MockMemory.Setup(m => m.GetValue(TestIValue + 1)).Returns(ExpectedRegister1Value).Verifiable();
            _testUtility.MockMemory.Setup(m => m.GetValue(TestIValue + 2)).Returns(ExpectedRegister2Value).Verifiable();
            _testUtility.MockMemory.Setup(m => m.GetValue(TestIValue + 3)).Returns(ExpectedRegister3Value).Verifiable();
            _testUtility.MockMemory.Setup(m => m.GetValue(TestIValue + 4)).Returns(ExpectedRegister4Value).Verifiable();
            _testUtility.MockMemory.Setup(m => m.GetValue(TestIValue + 5)).Returns(ExpectedRegister5Value).Verifiable();
            _testUtility.MockMemory.Setup(m => m.GetValue(TestIValue + 6)).Returns(ExpectedRegister6Value).Verifiable();
            _testUtility.MockMemory.Setup(m => m.GetValue(TestIValue + 7)).Returns(ExpectedRegister7Value).Verifiable();
            _testUtility.MockMemory.Setup(m => m.GetValue(TestIValue + 8)).Returns(ExpectedRegister8Value).Verifiable();
            _testUtility.MockMemory.Setup(m => m.GetValue(TestIValue + 9)).Returns(ExpectedRegister9Value).Verifiable();
            _testUtility.MockMemory.Setup(m => m.GetValue(TestIValue + 10)).Returns(ExpectedRegisterAValue).Verifiable();
            
            // Act
            _testUtility.TestCpu.EmulateOp();

            // Assert
            _testUtility.MockMemory.VerifyAll();
            Assert.AreEqual(ExpectedRegister0Value, _testUtility.MockRegisterBank.Object.V[0x00]);
            Assert.AreEqual(ExpectedRegister1Value, _testUtility.MockRegisterBank.Object.V[0x01]);
            Assert.AreEqual(ExpectedRegister2Value, _testUtility.MockRegisterBank.Object.V[0x02]);
            Assert.AreEqual(ExpectedRegister3Value, _testUtility.MockRegisterBank.Object.V[0x03]);
            Assert.AreEqual(ExpectedRegister4Value, _testUtility.MockRegisterBank.Object.V[0x04]);
            Assert.AreEqual(ExpectedRegister5Value, _testUtility.MockRegisterBank.Object.V[0x05]);
            Assert.AreEqual(ExpectedRegister6Value, _testUtility.MockRegisterBank.Object.V[0x06]);
            Assert.AreEqual(ExpectedRegister7Value, _testUtility.MockRegisterBank.Object.V[0x07]);
            Assert.AreEqual(ExpectedRegister8Value, _testUtility.MockRegisterBank.Object.V[0x08]);
            Assert.AreEqual(ExpectedRegister9Value, _testUtility.MockRegisterBank.Object.V[0x09]);
            Assert.AreEqual(ExpectedRegisterAValue, _testUtility.MockRegisterBank.Object.V[0x0a]);
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
                MockRegisterBank.Object.Stack = new Stack<ushort>(16);

                MockRandomService = new Mock<IRandomService>();
                MockDisplay = new Mock<IDisplay>();

                MockDisplay.SetupGet(d => d.Width).Returns(64);
                MockDisplay.SetupGet(d => d.Height).Returns(32);
                // Class under test instantiation
                TestCpu = new Cpu(MockMemory.Object, MockRegisterBank.Object, MockRandomService.Object,
                    MockDisplay.Object);
            }
            public Cpu TestCpu { get; private set; }

            public Mock<IMemory> MockMemory { get; private set; }
            public Mock<IRegisterBank> MockRegisterBank { get; private set; }
            public Mock<IRandomService> MockRandomService { get; private set; }
            public Mock<IDisplay> MockDisplay { get; private set; }

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

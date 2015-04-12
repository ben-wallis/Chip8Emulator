using System;

namespace Chip8Emulator
{
    internal class Cpu
    {
        private readonly IMemory _memory;
        private readonly IRegisterBank _registerBank;

        public Cpu(IMemory memory, IRegisterBank registerBank)
        {
            _memory = memory;
            _registerBank = registerBank;
            InitialiseRegisters();
        }

        private void InitialiseRegisters()
        {
        
        }

        public void EmulateOp()
        {
            var byte1Address = _registerBank.PC;
            var byte2Address = (ushort)(_registerBank.PC + 1);

            var opCode = new[] { _memory.GetValue(byte1Address), _memory.GetValue(byte2Address) };

            var highNib = opCode[0] >> 4;

            switch (highNib)
            {
                case 0x00:
                    throw new NotImplementedException("Unimplemented instruction");
                case 0x01: // 1NNN Jumps to address NNN
                {
                    var target = (ushort) (((opCode[0] & 0x0f) << 8) | opCode[1]);
                    _registerBank.PC = target;
                    break;
                }
                case 0x02: // 2NNN Calls subroutine at NNN
                    throw new NotImplementedException("Unimplemented instruction");
                case 0x03: // 3XNN Skips the next instruction if VX equals NN
                {
                    var reg = (ushort) (opCode[0] & 0x0f);
                    if (_registerBank.V[reg] == opCode[1])
                    {
                        _registerBank.PC += 2;
                    }

                    _registerBank.PC += 2;
                }
                    break;
                case 0x04: // 4XNN Skips the next instruction if VX doesn't equal NN
                {
                    var reg = (ushort)(opCode[0] & 0x0f);
                    if (_registerBank.V[reg] != opCode[1])
                    {
                        _registerBank.PC += 2;
                    }

                    _registerBank.PC += 2;
                    break;
                }
                case 0x05: // 5XY0 Skips the next instruction if VX equals VY
                {
                    var regX = (ushort)(opCode[0] & 0x0f);
                    var regY = (ushort)(opCode[1] >> 4);
                    if (_registerBank.V[regX] == _registerBank.V[regY])
                    {
                        _registerBank.PC += 2;
                    }

                    _registerBank.PC += 2;
                    break;
                }
                case 0x06: // 6XNN Sets VX to NN.
                {
                    var reg = (ushort) (opCode[0] & 0x0f);
                    _registerBank.V[reg] = opCode[1];

                    _registerBank.PC += 2;
                    break;
                }
                case 0x07: // 7XNN Adds NN to VX
                {
                    var reg = (ushort)(opCode[0] & 0x0f);
                    _registerBank.V[reg] += opCode[1];

                    _registerBank.PC += 2;
                    break;
                }
                case 0x08:
                {
                    var lastNibble = opCode[1] & 0x0f;
                    var regX = (ushort)(opCode[0] & 0x0f);
                    var regY = (ushort)(opCode[1] >> 4);

                    switch (lastNibble)
                    {
                        case 0x00:
                        {
                            _registerBank.V[regX] = _registerBank.V[regY];

                            _registerBank.PC += 2;
                            break;
                        }
                        case 0x01:
                        {
                            _registerBank.V[regX] = (byte) (_registerBank.V[regX] | _registerBank.V[regY]);

                            _registerBank.PC += 2;
                            break;
                        }
                        case 0x02:
                        {
                            _registerBank.V[regX] = (byte)(_registerBank.V[regX] & _registerBank.V[regY]);

                            _registerBank.PC += 2;
                            break;
                        }
                        case 0x03:
                        {
                            _registerBank.V[regX] = (byte) (_registerBank.V[regX] ^ _registerBank.V[regY]);

                            _registerBank.PC += 2;
                            break;
                        }
                        case 0x04:
                            throw new NotImplementedException("Unimplemented instruction 8XY4");
                        case 0x05:
                            throw new NotImplementedException("Unimplemented instruction 8XY5");
                        case 0x06:
                            throw new NotImplementedException("Unimplemented instruction 8XY6");
                        case 0x07:
                            throw new NotImplementedException("Unimplemented instruction 8XY7");
                        case 0x0e:
                            throw new NotImplementedException("Unimplemented instruction 8XYe");
                        default:
                            throw new NotImplementedException("Unimplemented instruction");
                    }
                    break;
                }
                case 0x09: // 9XY0 Skips the next instruction if VX doesn't equal VY
                {
                    var regX = (ushort) (opCode[0] & 0x0f);
                    var regY = (ushort) (opCode[1] >> 4);
                    if (_registerBank.V[regX] != _registerBank.V[regY])
                    {
                        _registerBank.PC += 2;
                    }

                    _registerBank.PC += 2;
                    break;
                }
                case 0x0a: // ANNN Sets I to the address NNN
                {
                    var address = (ushort)(((opCode[0] & 0x0f) << 8) | opCode[1]);
                    _registerBank.I = address;
                    _registerBank.PC += 2;
                    break;
                }
                case 0x0b: // BNNN Jumps to the address NNN plus V0
                    throw new NotImplementedException("Unimplemented instruction");
                case 0x0c: // CXNN Sets VX to a random number, masked by NN
                    throw new NotImplementedException("Unimplemented instruction");
                case 0x0d: // DXYN Sprites stored in memory at location in index register (I), maximum 8bits wide.
                           // Wraps around the screen. If when drawn, clears a pixel, register VF is set to 1 otherwise
                           // it is zero. All drawing is XOR drawing (i.e. it toggles the screen pixels)
                    throw new NotImplementedException("Unimplemented instruction");
                case 0x0e:
                    throw new NotImplementedException("Unimplemented instruction");
                case 0x0f:
                    throw new NotImplementedException("Unimplemented instruction");
                default:
                    throw new NotImplementedException("Unimplemented instruction");
            }
        }
    }
}

using System;
using Chip8Emulator.Services;

namespace Chip8Emulator
{
    internal class Cpu
    {
        private readonly IMemory _memory;
        private readonly IRegisterBank _registerBank;
        private readonly IRandomService _randomService;
        private readonly IDisplay _display;

        public Cpu(IMemory memory, IRegisterBank registerBank, IRandomService randomService, IDisplay display)
        {
            _memory = memory;
            _registerBank = registerBank;
            _randomService = randomService;
            _display = display;

            _registerBank.Initialise();
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
                {
                    switch (opCode[1])
                    {
                        case 0x00e0: // 00E0 Clears the screen
                            {
                                _display.Initialise();

                                _registerBank.PC += 2;
                                break;
                        }
                        case 0xee: // 00EE Returns from a subroutine
                        {
                            _registerBank.PC = _registerBank.Stack.Pop();
                            break;
                        }
                    }
                    break;
                }
                case 0x01: // 1NNN Jumps to address NNN
                {
                    var target = (ushort)(((opCode[0] & 0x0f) << 8) | opCode[1]);
                    _registerBank.PC = target;

                    break;
                }
                case 0x02: // 2NNN Calls subroutine at NNN
                {
                    _registerBank.Stack.Push(_registerBank.PC);

                    _registerBank.PC = (ushort)(((opCode[0] & 0x0f) << 8) | opCode[1]);
                    break;
                }
                case 0x03: // 3XNN Skips the next instruction if VX equals NN
                {
                    var regX = (byte)(opCode[0] & 0x0f);
                    if (_registerBank.V[regX] == opCode[1])
                    {
                        _registerBank.PC += 2;
                    }

                    _registerBank.PC += 2;
                    break;
                }
                case 0x04: // 4XNN Skips the next instruction if VX doesn't equal NN
                {
                    var regX = (byte)(opCode[0] & 0x0f);
                    if (_registerBank.V[regX] != opCode[1])
                    {
                        _registerBank.PC += 2;
                    }

                    _registerBank.PC += 2;
                    break;
                }
                case 0x05: // 5XY0 Skips the next instruction if VX equals VY
                {
                    var regX = (byte)(opCode[0] & 0x0f);
                    var regY = (byte)(opCode[1] >> 4);
                    if (_registerBank.V[regX] == _registerBank.V[regY])
                    {
                        _registerBank.PC += 2;
                    }

                    _registerBank.PC += 2;
                    break;
                }
                case 0x06: // 6XNN Sets VX to NN.
                {
                    var regX = (byte)(opCode[0] & 0x0f);
                    _registerBank.V[regX] = opCode[1];

                    _registerBank.PC += 2;
                    break;
                }
                case 0x07: // 7XNN Adds NN to VX
                {
                    var regX = (byte)(opCode[0] & 0x0f);
                    _registerBank.V[regX] += opCode[1];

                    _registerBank.PC += 2;
                    break;
                }
                case 0x08:
                {
                    var lastNibble = opCode[1] & 0x0f;
                    var regX = (byte)(opCode[0] & 0x0f);
                    var regY = (byte)(opCode[1] >> 4);

                    switch (lastNibble)
                    {
                        case 0x00: // 8XY0 Sets VX to the value of VY
                        {
                            _registerBank.V[regX] = _registerBank.V[regY];

                            _registerBank.PC += 2;
                            break;
                        }
                        case 0x01: // 8XY1 Sets VX to VX or VY
                        {
                            _registerBank.V[regX] |= _registerBank.V[regY];

                            _registerBank.PC += 2;
                            break;
                        }
                        case 0x02: // 8XY2 Sets VX to VX and VY
                        {
                            _registerBank.V[regX] &= _registerBank.V[regY];

                            _registerBank.PC += 2;
                            break;
                        }
                        case 0x03: // 8XY3 Sets VX to VX xor VY
                        {
                            _registerBank.V[regX] ^= _registerBank.V[regY];

                            _registerBank.PC += 2;
                            break;
                        }
                        case 0x04: // 8XY4 Adds VY to VX. VF is set to 1 when there's a carry, and to 0 when there isn't
                        {
                            var result = _registerBank.V[regX] + _registerBank.V[regY];
                            _registerBank.V[regX] = (byte) (result%byte.MaxValue);

                            if (result > byte.MaxValue)
                            {
                                _registerBank.V[0x0f] = 0x01;
                            }

                            _registerBank.PC += 2;
                            break;
                        }
                        case 0x05:
                            // 8XY5 VY is subtracted from VX. VF is set to 0 when there's a borrow, and 1 when there isn't
                        {
                            var result = _registerBank.V[regX] - _registerBank.V[regY];
                            _registerBank.V[0x0f] = (byte) (result < 0 ? 0 : 1);
                            _registerBank.V[regX] = (byte) result;

                            _registerBank.PC += 2;
                            break;
                        }
                        case 0x06:
                            // 8XY6 Shifts VX right by one. VF is set to the value of the least significant bit of VX before the shift
                        {
                            _registerBank.V[0x0f] = (byte) (_registerBank.V[regX] & 0x0f);
                            _registerBank.V[regX] = (byte) (_registerBank.V[regX] >> 4);

                            _registerBank.PC += 2;
                            break;
                        }
                        case 0x07:
                            // 8XY7 Sets VX to VY minus VX. VF is set to 0 when there's a borrow, and 1 when there isn't
                        {
                            var result = _registerBank.V[regY] - _registerBank.V[regX];
                            _registerBank.V[0x0f] = (byte) (result < 0 ? 0 : 1);
                            _registerBank.V[regX] = (byte) result;

                            _registerBank.PC += 2;
                            break;
                        }
                        case 0x0e:
                            // 8XYE Shifts VX left by one. VF is set to the value of the most significant bit of VX before the shift
                        {
                            _registerBank.V[0x0f] = (byte) (_registerBank.V[regX] >> 4);
                            _registerBank.V[regX] = (byte) (_registerBank.V[regX] << 4);

                            _registerBank.PC += 2;
                            break;
                        }
                        default:
                        {
                            throw new NotImplementedException(String.Format("Unimplemented instruction {0:x2}{1:x2}",
                                opCode[0], opCode[1]));
                        }
                    }
                    break;
                }
                case 0x09: // 9XY0 Skips the next instruction if VX doesn't equal VY
                {
                    var regX = (byte)(opCode[0] & 0x0f);
                    var regY = (byte)(opCode[1] >> 4);
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
                {
                    var target = (ushort)(((opCode[0] & 0x0f) << 8) | opCode[1]);
                    _registerBank.PC = (ushort)(target + _registerBank.V[0]);

                    break;
                }
                case 0x0c: // CXNN Sets VX to a random number, masked by NN
                {
                    var regX = (byte)(opCode[0] & 0x0f);
                    _registerBank.V[regX] = (byte)(_randomService.GetRandomByte() & opCode[1]);

                    _registerBank.PC += 2;
                    break;
                }
                case 0x0d: // DXYN Sprites stored in memory at location in index register (I), maximum 8bits wide.
                           // Wraps around the screen. If when drawn, clears a pixel, register VF is set to 1 otherwise
                           // it is zero. All drawing is XOR drawing (i.e. it toggles the screen pixels)
                {
                    var regX = (byte)(opCode[0] & 0x0f);
                    var regY = (byte)(opCode[1] >> 4);
                    var spriteHeight = (byte)(opCode[1] & 0x0f);
                    var coordX = _registerBank.V[regX];
                    var coordY = _registerBank.V[regY];
                    var displayWidth = _display.Width;
                    var displayHeight = _display.Height;

                    // TODO: Refactor this mess.
                    var memPos = 0;
                    for (var y = coordY; y < coordY + spriteHeight && y < displayHeight; y++)
                    {
                        var line = _memory.GetValue((ushort) (_registerBank.I + memPos++));
                        for (var x = 0; x < 8; x++)
                        {
                            if (coordX + (7 - x) > (displayWidth - 1)) continue;

                            var result = (line >> x) & 0x1;
                            if (result != 0x01) continue;

                            if (_display.FlipPixel((byte) (coordX + (7 - x)), y))
                            {
                                _registerBank.V[0x0f] = 0x01;
                            }
                        }
                    }

                    _registerBank.PC += 2;
                    break;
                }
                case 0x0e:
                {
                    var regX = (byte)(opCode[0] & 0x0f);

                    switch (opCode[1])
                    {
                        case 0x9e: // EX9E Skips the next instruction if the key stored in VX is pressed
                        {
                            if (_registerBank.KeyPressed && _registerBank.Key == _registerBank.V[regX])
                            {
                                _registerBank.PC += 2;
                            }

                            _registerBank.PC += 2;
                            break;
                        }
                        case 0xa1: // EX1A Skips the next instruction if the key stored in VX isn't pressed
                        {
                            if (!_registerBank.KeyPressed || (_registerBank.KeyPressed && _registerBank.Key != _registerBank.V[regX]))
                            {
                                _registerBank.PC += 2;
                            }

                            _registerBank.PC += 2;
                            break;
                        }
                        default:
                        {
                            throw new NotImplementedException(String.Format("Unimplemented instruction {0:x2}{1:x2}",
                                opCode[0], opCode[1]));
                        }
                    }
                    break;
                }
                case 0x0f:
                    {
                        var regX = (byte)(opCode[0] & 0x0f);

                        switch (opCode[1])
                        {
                            case 0x07: // FX07 Sets VX to the value of the delay timer
                            {
                                _registerBank.V[regX] = _registerBank.Delay;

                                _registerBank.PC += 2;
                                break;
                            }
                            case 0x0a: // FX0A A key press is awaited, and then stored in VX
                            {
                                if (_registerBank.KeyPressed)
                                {
                                    _registerBank.V[regX] = _registerBank.Key;
                                    _registerBank.PC += 2;
                                }

                                break;
                            }
                            case 0x15: // FX15 Sets the delay timer to VX
                            {
                                _registerBank.Delay = _registerBank.V[regX];

                                _registerBank.PC += 2;
                                break;
                            }
                            case 0x18: // FX18 Sets the sound timer to VX
                            {
                                _registerBank.Sound = _registerBank.V[regX];

                                _registerBank.PC += 2;
                                break;
                            }
                            case 0x1e: // FX1E Adds VX to I
                            {
                                _registerBank.I += _registerBank.V[regX];

                                _registerBank.PC += 2;
                                break;
                            }
                            case 0x29: // FX29 Sets I to the location of the sprite for the character in VX. Characters 0-F (in hexadecimal) are represented by a 4x5 font
                            {
                                _registerBank.I = (ushort)(_registerBank.V[regX] * 5); // Default font sprites start at memory address 0x00 and are 5 bytes each

                                _registerBank.PC += 2;
                                break;
                            }
                            case 0x33: // FX33 Stores the Binary-coded decimal representation of VX, with the most significant of three digits at the address in I,
                                       // the middle digit at I plus 1, and the least significant digit at I plus 2. (In other words, take the decimal
                                       // representation of VX, place the hundreds digit in memory at location in I, the tens digit at location I+1, 
                                       // and the ones digit at location I+2.)
                            {
                                var value = _registerBank.V[regX];
                                var ones = value%10;
                                value /= 10;
                                var tens = value%10;
                                var hundreds = value/10;

                                _memory.SetValue(_registerBank.I, (byte)hundreds);
                                _memory.SetValue((ushort)(_registerBank.I + 1), (byte)tens);
                                _memory.SetValue((ushort)(_registerBank.I + 2), (byte)ones);
                                
                                _registerBank.PC += 2;
                                break;
                            }
                            case 0x55: // FX55 Stores V0 to VX in memory starting at address I
                            {
                                for (var register = 0; register <= regX; register++)
                                {
                                    _memory.SetValue((ushort)(_registerBank.I + register), _registerBank.V[register]);
                                }
                                
                                _registerBank.PC += 2;
                                break;
                            }
                            case 0x65: // FX65 Fills V0 to VX with values from memory starting at address I
                            {
                                for (var register = 0; register <= regX; register++)
                                {
                                    _registerBank.V[register] = _memory.GetValue((ushort)(_registerBank.I + register));
                                }

                                _registerBank.PC += 2;
                                break;
                            }
                            default:
                            {
                                throw new NotImplementedException(String.Format(
                                    "Unimplemented instruction {0:x2}{1:x2}",
                                    opCode[0], opCode[1]));
                            }
                        }
                        break;
                    }
                default:
                {
                    throw new NotImplementedException(String.Format("Unimplemented instruction {0:x2}{1:x2}", opCode[0],
                        opCode[1]));
                }
            }
        }
    }
}

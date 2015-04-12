using System;

namespace Chip8Emulator
{
    internal class Disassembler : IDisassembler
    {
        private readonly IMemory _memory;
        private readonly IFileHandler _fileHandler;

        public Disassembler(IMemory memory, IFileHandler fileHandler)
        {
            _memory = memory;
            _fileHandler = fileHandler;
        }

        public void DisassembleFile(string filePath)
        {
            var fileLength = _fileHandler.LoadFileIntoMemory(filePath);
            ushort programCounter = 0x200;

            while (programCounter < 0x200 + fileLength)
            {
                DisassembleChip8OpCode(programCounter);
                programCounter += 2;
            }
        }

        private void DisassembleChip8OpCode(ushort startAddress)
        {
            var byte1Address = startAddress;
            var byte2Address = (ushort) (startAddress + 1);

            var opCode = new [] { _memory.GetValue(byte1Address), _memory.GetValue(byte2Address) };

            var opCodeFirstNibble = opCode[0] >> 4;
            var opCodeSecondNibble = opCode[0] & 0x0f;
            var opCodeThirdNibble = opCode[1] >> 4;
            var opCodeLastNibble = opCode[1] & 0x0f;

            Console.Write("{0:X8} {1:x2}{2:x2} ", startAddress, opCode[0], opCode[1]);

            switch (opCodeFirstNibble)
            {
                case 0x00:
                    Console.WriteLine("NOP");
                    break;
                case 0x01: // 1NNN Jumps to address NNN
                    Console.WriteLine("JMP       #${0:x}{1:x2}", opCodeSecondNibble, opCode[1]);
                    break;
                case 0x02: // 2NNN Calls subroutine at NNN
                    Console.WriteLine("CALL      #${0:x}{1:x2}", opCodeSecondNibble, opCode[1]);
                    break;
                case 0x03: // 3XNN Skips the next instruction if VX equals NN
                    Console.WriteLine("SKIP.EQ   V{0:X}, {1:x2}", opCodeSecondNibble, opCode[1]);
                    break;
                case 0x04: // 4XNN Skips the next instruction if VX doesn't equal NN
                    Console.WriteLine("SKIP.NE   V{0:X}, {1:x2}", opCodeSecondNibble, opCode[1]);
                    break;
                case 0x05: // 5XY0 Skips the next instruction if VX equals VY
                    Console.WriteLine("SKIP.EQ   V{0:X}, V{1:x}", opCodeSecondNibble, opCodeThirdNibble);
                    break;
                case 0x06: // 6XNN Sets VX to NN.
                    Console.WriteLine("MVI       V{0:X}, {1:x2}", opCodeSecondNibble, opCode[1]);
                    break;
                case 0x07: // 7XNN Adds NN to VX
                    Console.WriteLine("ADD       V{0:X}, {1:x2}", opCodeSecondNibble, opCode[1]);
                    break;
                case 0x08:
                    switch (opCodeLastNibble)
                    {
                        case 0x00: // 8XY0 Sets VX to the value of VY
                            Console.WriteLine("MVI       V{0:X}, V{1:x}", opCodeSecondNibble, opCodeThirdNibble);
                            break;
                        case 0x01: // 8XY1 Sets VX to VX or VY
                            Console.WriteLine("OR        V{0:X}, V{1:x}", opCodeSecondNibble, opCodeThirdNibble);
                            break;
                        case 0x02: // 8XY2 Sets VX to VX and VY
                            Console.WriteLine("AND       V{0:X}, V{1:x}", opCodeSecondNibble, opCodeThirdNibble);
                            break;
                        case 0x03: // 8XY3 Sets VX to VX xor VY
                            Console.WriteLine("XOR       V{0:X}, V{1:x}", opCodeSecondNibble, opCodeThirdNibble);
                            break;
                        case 0x04: // 8XY4 Adds VY to VX. VF is set to 1 when there's a carry, and to 0 when there isn't
                            Console.WriteLine("ADD       V{0:X}, V{1:x}", opCodeSecondNibble, opCodeThirdNibble);
                            break;
                        case 0x05: // 8YX5 VY is subtracted from VX. VF is set to 0 when there's a borrow, and 1 when there isn't
                            Console.WriteLine("SUB       V{0:X}, V{1:x}", opCodeSecondNibble, opCodeThirdNibble);
                            break;
                        case 0x06: // 8XY6 Shifts VX right by one. VF is set to the value of the least significant bit of VX before the shift
                            Console.WriteLine("SHR       V{0:X}", opCodeSecondNibble);
                            break;
                        case 0x07: // 8XY7 Sets VX to VY minus VX. VF is set to 0 when there's a borrow, and 1 when there isn't
                            Console.WriteLine("SUBB      V{0:X}, V{1:x}", opCodeSecondNibble, opCodeThirdNibble);
                            break;
                        case 0x0e: // 8XYE Shifts VX left by one. VF is set to the value of the most significant bit of VX before the shift
                            Console.WriteLine("SHL       V{0:X}", opCodeSecondNibble, opCodeThirdNibble);
                            break;
                        default:
                            Console.WriteLine("8XY{0:X} not implemented", opCodeLastNibble);
                            break;
                    }
                    break;
                case 0x09: // 9XY0 Skips the next instruction if VX doesn't equal VY
                    Console.WriteLine("SKIP.NE   V{0:X}, V{1:X}", opCodeSecondNibble, opCodeThirdNibble);
                    break;
                case 0x0a: // ANNN Sets I to the address NNN
                    Console.WriteLine("MVI       I  #${0:x}{1:x2}", opCodeSecondNibble, opCode[1]);
                    break;
                case 0x0b: // BNNN Jumps to the address NNN plus V0
                    Console.WriteLine("JMP       #${0:x}{1:x2}(V0)", opCodeSecondNibble, opCode[1]);
                    break;
                case 0x0c: // CXNN Sets VX to a random number, masked by NN
                    Console.WriteLine("RNDMASK   V{0:X} {1:x2}", opCodeSecondNibble, opCode[1]);
                    break;
                case 0x0d: // DXYN Sprites stored in memory at location in index register (I), maximum 8bits wide.
                           // Wraps around the screen. If when drawn, clears a pixel, register VF is set to 1 otherwise
                           // it is zero. All drawing is XOR drawing (i.e. it toggles the screen pixels)
                    Console.WriteLine("SPRITE    V{0:X} V{1:X} #${2:x}", opCodeSecondNibble, opCodeThirdNibble, opCodeLastNibble);
                    break;
                case 0x0e:
                    switch (opCode[1])
                    {
                        case 0x9e: // EX9E Skips the next instruction if the key stored in VX is pressed
                            Console.WriteLine("SKIPKEY.Y V{0:X}", opCodeSecondNibble);
                            break;
                        case 0xa1: // EXA1 Skips the next instruction if the key stored in VX isn't pressed
                            Console.WriteLine("SKIPKEY.N V{0:X}", opCodeSecondNibble);
                            break;
                        default:
                            Console.WriteLine("EX{0:X2} not implemented", opCode[1]);
                            break;
                    }
                    break;
                case 0x0f:
                    switch (opCode[1])
                    {
                        case 0x07: // FX07 Sets VX to the value of the delay timer
                            Console.WriteLine("MOV       V{0:X}, DELAY", opCodeSecondNibble);
                            break;
                        case 0x0a: // FX0A A key press is awaited, and then stored in VX
                            Console.WriteLine("WAITKEY   V{0:X}", opCodeSecondNibble);
                            break;
                        case 0x15: // FX15 Sets the delay timer to VX
                            Console.WriteLine("MOV       DELAY, V{0:X}", opCodeSecondNibble);
                            break;
                        case 0x18: // FX18 Sets the sound timer to VX
                            Console.WriteLine("MOV       SOUND, V{0:X}", opCodeSecondNibble);
                            break;
                        case 0x1e: // FX1E Adds VX to I
                            Console.WriteLine("ADD       I, V{0:X}", opCodeSecondNibble);
                            break;
                        case 0x29: // FX29 Sets I to the location of the sprite for the character in VX. Characters 0-F (in hexadecimal) are
                                   // represented by a 4x5 font
                            Console.WriteLine("SPRITECHAR V{0:X}", opCodeSecondNibble);
                            break;
                        case 0x33: // FX33 Stores the Binary-coded decimal representation of VX, with the most significant of three digits
                                   // at the address in I, the middle digit at I plus 1, and the least significant digit at I plus 2.
                                   // (In other words, take the decimal representation of VX, place the hundreds digit in memory at location in I,
                                   // the tens digit at location I+1, and the ones digit at location I+2.)
                            Console.WriteLine("MOVBCD    V{0:X}", opCodeSecondNibble);
                            break;
                        case 0x55: // FX55 Stores V0 to VX in memory starting at address I
                            Console.WriteLine("MOVM      (I), V0-V{0:X}", opCodeSecondNibble);
                            break;
                        case 0x65: // FX65 Fills V0 to VX with values from memory starting at address I
                            Console.WriteLine("MOVM      V0-V{0:X}, (I)", opCodeSecondNibble);
                            break;
                        default:
                            Console.WriteLine("FX{0:X2} not implemented", opCode[1]);
                            break;
                    }
                    break;
                default:
                    Console.WriteLine("Unknown OpCode first nibble");
                    break;
            }
        }
    }
}

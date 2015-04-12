using System;

namespace Chip8Emulator
{
    public class Cpu
    {
        private readonly IMemory _memory;

        public ushort SP { get; private set; }
        public ushort PC { get; private set; }
        public ushort I { get; private set; }
        public byte[] V { get; private set; }
        public byte Delay { get; private set; }
        public byte Sound { get; private set; }
        
        public Cpu(IMemory memory)
        {
            _memory = memory;
            Initialise();
        }

        private void Initialise()
        {
            V = new byte[16]; // Initialise Registers
            PC = 0x200;
            SP = 0xfa0;
        }

        public void EmulateOp()
        {
            var byte1Address = PC;
            var byte2Address = (ushort)(PC + 1);

            var opCode = new[] { _memory.GetValue(byte1Address), _memory.GetValue(byte2Address) };

            var highNib = opCode[0] >> 4;

            switch (highNib)
            {
                case 0x00:
                    throw new NotImplementedException("Unimplemented instruction");
                case 0x01: // 1NNN Jumps to address NNN
                {
                    var target = (ushort) (opCode[0] & 0x0f | opCode[1]);
                    PC = target;
                    break;
                }
                case 0x02: // 2NNN Calls subroutine at NNN
                    throw new NotImplementedException("Unimplemented instruction");
                case 0x03: // 3XNN Skips the next instruction if VX equals NN
                {
                    throw new NotImplementedException("Unimplemented instruction");
                }
                    break;
                case 0x04: // 4XNN Skips the next instruction if VX doesn't equal NN
                    throw new NotImplementedException("Unimplemented instruction");
                case 0x05: // 5XY0 Skips the next instruction if VX equals VY
                    throw new NotImplementedException("Unimplemented instruction");
                case 0x06: // 6XNN Sets VX to NN.
                    throw new NotImplementedException("Unimplemented instruction");
                case 0x07: // 7XNN Adds NN to VX
                    throw new NotImplementedException("Unimplemented instruction");
                case 0x08: 
                    throw new NotImplementedException("Unimplemented instruction");
                case 0x09: // 9XY0 Skips the next instruction if VX doesn't equal VY
                    throw new NotImplementedException("Unimplemented instruction");
                case 0x0a: // ANNN Sets I to the address NNN
                    throw new NotImplementedException("Unimplemented instruction");
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

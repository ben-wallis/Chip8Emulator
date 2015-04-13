using System.Collections.Generic;

namespace Chip8Emulator.Core
{
    internal class RegisterBank : IRegisterBank
    {
        public ushort SP { get; set; }
        public ushort PC { get; set; }
        public ushort I { get; set; }
        public byte[] V { get; set; }
        public byte Key { get; set; }
        public bool KeyPressed { get; set; }
        public byte Delay { get; set; }
        public byte Sound { get; set; }
        public Stack<ushort> Stack { get; set; }

        public void Initialise()
        {
            V = new byte[16];
            PC = 0x200;
            SP = 0xfa0;
            Stack = new Stack<ushort>(16);
        }
    }
}

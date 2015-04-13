using System.Collections.Generic;

namespace Chip8Emulator.Core
{
    internal interface IRegisterBank
    {
        ushort SP { get; set; }
        ushort PC { get; set; }
        ushort I { get; set; }
        byte[] V { get; set; }
        byte Key { get; set; }
        bool KeyPressed { get; set; }
        byte Delay { get; set; }
        byte Sound { get; set; }
        Stack<ushort> Stack { get; set; }

        void Initialise();
    }
}
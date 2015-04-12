namespace Chip8Emulator
{
    internal class RegisterBank : IRegisterBank
    {
        public ushort SP { get; set; }
        public ushort PC { get; set; }
        public ushort I { get; set; }
        public byte[] V { get; set; }
        public byte Delay { get; set; }
        public byte Sound { get; set; }

        public void Initialise()
        {
            V = new byte[16];
            PC = 0x200;
            SP = 0xfa0;
        }
    }
}

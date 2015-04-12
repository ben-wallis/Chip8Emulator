namespace Chip8Emulator
{
    internal interface IRegisterBank
    {
        ushort SP { get; set; }
        ushort PC { get; set; }
        ushort I { get; set; }
        byte[] V { get; set; }
        byte Delay { get; set; }
        byte Sound { get; set; }
    }
}
namespace Chip8Emulator
{
    internal interface IMemory
    {
        void SetValue(ushort address, byte value);
        byte GetValue(ushort address);
        void LoadProgram(byte[] program);
    }
}
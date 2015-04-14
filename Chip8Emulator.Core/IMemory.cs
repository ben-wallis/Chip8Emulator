namespace Chip8Emulator.Core
{
    internal interface IMemory
    {
        void SetValue(ushort address, byte value);
        byte GetValue(ushort address);
        void LoadProgram(byte[] program);
    }
}
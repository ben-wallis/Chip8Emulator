namespace Chip8Emulator.Core
{
    public interface IMemory
    {
        void SetValue(ushort address, byte value);
        byte GetValue(ushort address);
        void LoadProgram(byte[] program);
    }
}
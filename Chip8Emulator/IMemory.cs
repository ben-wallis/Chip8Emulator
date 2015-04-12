namespace Chip8Emulator
{
    public interface IMemory
    {
        void SetValue(ushort address, byte value);
        byte GetValue(ushort address);
        void LoadProgram(byte[] program);
    }
}
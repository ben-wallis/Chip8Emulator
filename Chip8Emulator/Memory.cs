namespace Chip8Emulator
{
    public class Memory : IMemory
    {
        private const ushort MemorySize = 4096;
        private const ushort ProgramStart = 0x200;

        readonly byte[] _memoryBuffer = new byte[MemorySize];
        
        public void SetValue(ushort address, byte value)
        {
            _memoryBuffer[address] = value;
        }

        public byte GetValue(ushort address)
        {
            return _memoryBuffer[address];
        }

        public void LoadProgram(byte[] program)
        {
            program.CopyTo(_memoryBuffer, ProgramStart);
        }
    }
}

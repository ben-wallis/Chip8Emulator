namespace Chip8Emulator
{
    internal class Memory : IMemory
    {
        private const ushort MemorySize = 4096;
        private const ushort ProgramStart = 0x200;

        private readonly byte[] _memoryBuffer = new byte[MemorySize];

        public Memory()
        {
            LoadDefaultFontSprites();    
        }

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

        // TODO: Put this somewhere else, doesn't really belong in here.
        // TODO: Create LoadAtAddress method to faciliate loading of sprites etc. externally.
        private void LoadDefaultFontSprites()
        {
            var sprites = new byte[16][];
            sprites[0x00] = new byte[] { 0xf0, 0x90, 0x90, 0x90, 0xf0 };
            sprites[0x01] = new byte[] { 0x20, 0x60, 0x20, 0x20, 0x70 };
            sprites[0x02] = new byte[] { 0xf0, 0x10, 0xf0, 0x80, 0xf0 };
            sprites[0x03] = new byte[] { 0xf0, 0x10, 0xf0, 0x10, 0xf0 };
            sprites[0x04] = new byte[] { 0x90, 0x90, 0xf0, 0x10, 0x10 };
            sprites[0x05] = new byte[] { 0xf0, 0x80, 0xf0, 0x10, 0xf0 };
            sprites[0x06] = new byte[] { 0xf0, 0x80, 0xf0, 0x90, 0xf0 };
            sprites[0x07] = new byte[] { 0xf0, 0x10, 0x20, 0x40, 0x40 };
            sprites[0x08] = new byte[] { 0xf0, 0x90, 0xf0, 0x90, 0xf0 };
            sprites[0x09] = new byte[] { 0xf0, 0x90, 0xf0, 0x10, 0xf0 };
            sprites[0x0a] = new byte[] { 0xf0, 0x90, 0xd0, 0x90, 0x90 };
            sprites[0x0b] = new byte[] { 0xe0, 0x90, 0xe0, 0x90, 0xe0 };
            sprites[0x0c] = new byte[] { 0xf0, 0x80, 0x80, 0x80, 0xf0 };
            sprites[0x0d] = new byte[] { 0xe0, 0x90, 0x90, 0x90, 0xe0 };
            sprites[0x0e] = new byte[] { 0xf0, 0x80, 0xf0, 0x80, 0xf0 };
            sprites[0x0f] = new byte[] { 0xf0, 0x80, 0xf0, 0x80, 0x80 };

            for (var i = 0x0; i <= 0x0f; i++)
            {
                sprites[i].CopyTo(_memoryBuffer, i * 5);
            }
        }
    }
}

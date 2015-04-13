namespace Chip8Emulator.Core
{
    internal interface IDisassembler
    {
        void DisassembleFile(string filePath);
    }
}
namespace Chip8Emulator.Core
{
    internal interface IFileHandler
    {
        int LoadFileIntoMemory(string filePath);
    }
}
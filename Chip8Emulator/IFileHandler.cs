namespace Chip8Emulator
{
    internal interface IFileHandler
    {
        int LoadFileIntoMemory(string filePath);
    }
}
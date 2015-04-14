namespace Chip8Emulator.Core
{
    public interface IFileHandler
    {
        int LoadFileIntoMemory(string filePath);
    }
}
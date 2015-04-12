namespace Chip8Emulator
{
    public interface IFileHandler
    {
        int LoadFileIntoMemory(string filePath);
    }
}
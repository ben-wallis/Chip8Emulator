namespace Chip8Emulator.Services
{
    public interface IFileSystemService
    {
        byte[] ReadFileAsByteArray(string filePath);
    }
}
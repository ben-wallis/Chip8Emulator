namespace Chip8Emulator.Services
{
    internal interface IFileSystemService
    {
        byte[] ReadFileAsByteArray(string filePath);
    }
}
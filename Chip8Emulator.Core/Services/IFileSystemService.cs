namespace Chip8Emulator.Core.Services
{
    internal interface IFileSystemService
    {
        byte[] ReadFileAsByteArray(string filePath);
    }
}
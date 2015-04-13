using System.IO;

namespace Chip8Emulator.Core.Services
{
    internal class FileSystemService : IFileSystemService
    {
        public byte[] ReadFileAsByteArray(string filePath)
        {
            return File.ReadAllBytes(filePath);
        }
    }
}

using System.IO;

namespace Chip8Emulator.Services
{
    public class FileSystemService : IFileSystemService
    {
        public byte[] ReadFileAsByteArray(string filePath)
        {
            return File.ReadAllBytes(filePath);
        }
    }
}

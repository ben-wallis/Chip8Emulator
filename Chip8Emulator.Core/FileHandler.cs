﻿using Chip8Emulator.Core.Services;

namespace Chip8Emulator.Core
{
    internal class FileHandler : IFileHandler
    {
        private readonly IMemory _memory;
        private readonly IFileSystemService _fileSystemService;

        public FileHandler(IMemory memory, IFileSystemService fileSystemService)
        {
            _memory = memory;
            _fileSystemService = fileSystemService;
        }

        public int LoadFileIntoMemory(string filePath)
        {
            var fileByteArray = _fileSystemService.ReadFileAsByteArray(filePath);
            _memory.LoadProgram(fileByteArray);

            return fileByteArray.Length;
        }
    }
}

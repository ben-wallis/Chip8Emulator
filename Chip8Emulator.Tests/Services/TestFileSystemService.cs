using Chip8Emulator.Core.Services;
using Chip8Emulator.Core.Services;
using NUnit.Framework;

namespace Chip8Emulator.Core.Core.Tests.Services
{
    [TestFixture]
    public class TestFileSystemService
    {
        [Test]
        public void ReadFileAsByteArray_ReadsFileAsByteArray()
        {
            // Arrange
            var fileSystemService = new FileSystemService();
            const string TestInputPath = @"C:\TestFile.dat"; //Contains "hello"
            
            // Act
            var result = fileSystemService.ReadFileAsByteArray(TestInputPath);

            // Assert
            byte[] expectedResult = {0x48, 0x65, 0x6c, 0x6c, 0x6f};
            Assert.AreEqual(result, expectedResult);
        }
    }
}

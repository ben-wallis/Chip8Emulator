using Chip8Emulator.Core;
using Chip8Emulator.Core.Services;
using Moq;
using NUnit.Framework;

namespace Chip8Emulator.Core.Tests
{
    [TestFixture]
    public class TestFileHandler
    {
        [Test]
        public void LoadFileIntoMemory_LoadsFileIntoMemory()
        {
            // Arrange
            const string TestFilePath = "c:\\abc.dat";
            var testBytes = new byte[] { 0x0 };

            var mockMemory = new Mock<IMemory>();
            mockMemory.Setup(m => m.LoadProgram(testBytes)).Verifiable();

            var mockFileSystemService = new Mock<IFileSystemService>();
            mockFileSystemService.Setup(f => f.ReadFileAsByteArray(TestFilePath)).Returns(testBytes).Verifiable();
            
            var fileHandler = new FileHandler(mockMemory.Object, mockFileSystemService.Object);
            
            // Act
            fileHandler.LoadFileIntoMemory(TestFilePath);

            // Assert
            mockMemory.Verify();
            mockFileSystemService.Verify();
        }

        [Test]
        public void LoadFileIntoMemory_ReturnsFileLength()
        {
            // Arrange
            const string TestFilePath = "c:\\abc.dat";
            var testBytes = new byte[] { 0x0, 0x0, 0x0, 0x0, 0x0 };

            var mockMemory = new Mock<IMemory>();
            mockMemory.Setup(m => m.LoadProgram(testBytes)).Verifiable();

            var mockFileSystemService = new Mock<IFileSystemService>();
            mockFileSystemService.Setup(f => f.ReadFileAsByteArray(TestFilePath)).Returns(testBytes).Verifiable();

            var fileHandler = new FileHandler(mockMemory.Object, mockFileSystemService.Object);

            // Act
            var result = fileHandler.LoadFileIntoMemory(TestFilePath);

            // Assert
            Assert.AreEqual((int) 5, (int) result);
        }
    }
}

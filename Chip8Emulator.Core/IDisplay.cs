namespace Chip8Emulator.Core
{
    public interface IDisplay
    {
        bool[,] Pixels { get; }
        void Initialise();
        ushort Height { get; }
        ushort Width { get; }
        void DumpToConsole();
        bool FlipPixel(byte x, byte y);
    }
}
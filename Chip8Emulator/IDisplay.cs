namespace Chip8Emulator
{
    internal interface IDisplay
    {
        bool[,] Pixels { get; }
        void Initialise();
        ushort Height { get; }
        ushort Width { get; }
        bool FlipPixel(byte x, byte y);
    }
}
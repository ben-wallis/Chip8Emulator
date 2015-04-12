namespace Chip8Emulator
{
    internal interface IDisplay
    {
        bool[,] Pixels { get; }
        void Initialise();
        void SetPixel(byte x, byte y, bool value);
    }
}
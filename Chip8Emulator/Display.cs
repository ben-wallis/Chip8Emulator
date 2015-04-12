namespace Chip8Emulator
{
    internal class Display : IDisplay
    {
        public bool[,] Pixels { get; private set; }

        public Display()
        {
            Initialise();
        }

        public void Initialise()
        {
            Pixels = new bool[64, 32];
        }

        public ushort Height
        {
            get { return 32; }
        }

        public ushort Width
        {
            get { return 64; }
        }

        public bool FlipPixel(byte x, byte y)
        {
            var existingValue = Pixels[x, y];

            Pixels[x, y] ^= true;

            return existingValue;
        }
    }
}

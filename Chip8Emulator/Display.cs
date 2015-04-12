namespace Chip8Emulator
{
    internal class Display : IDisplay
    {
        private bool[,] _displayPixels;

        public bool[,] Pixels
        {
            get { return _displayPixels; }
        }

        public Display()
        {
            Initialise();
        }

        public void Initialise()
        {
            _displayPixels = new bool[64, 32];
        }

        public void SetPixel(byte x, byte y, bool value)
        {
            _displayPixels[x, y] = value;
        }
    }
}

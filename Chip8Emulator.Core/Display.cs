using System;

namespace Chip8Emulator.Core
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

        public void DumpToConsole()
        {
            for (var y = 0; y < Height; y++)
            {
                var line = "";
                for (var x = 0; x < Width; x++)
                {
                    line = line + (Pixels[x, y] ? "█" : " ");
                }
                Console.WriteLine(line);
                Console.WriteLine();
            }
        }

        public bool FlipPixel(byte x, byte y)
        {
            var existingValue = Pixels[x, y];

            Pixels[x, y] ^= true;

            return existingValue;
        }
    }
}

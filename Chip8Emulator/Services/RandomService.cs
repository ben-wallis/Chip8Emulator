using System;

namespace Chip8Emulator.Services
{
    internal class RandomService : IRandomService
    {
        private readonly Random _random;

        public RandomService()
        {
            _random = new Random();
        }

        public byte GetRandomByte()
        {
            return (byte)_random.Next(0, 255);
        }
    }
}

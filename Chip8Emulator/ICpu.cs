namespace Chip8Emulator.Core
{
    internal interface ICpu
    {
        bool DrawRequired { get; }
        void EmulateOp();
    }
}
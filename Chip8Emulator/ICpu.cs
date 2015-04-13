namespace Chip8Emulator
{
    internal interface ICpu
    {
        bool DrawRequired { get; }
        void EmulateOp();
    }
}
namespace Chip8Emulator.Core
{
    public interface ICpu
    {
        bool DrawRequired { get; }
        void EmulateOp();
    }
}
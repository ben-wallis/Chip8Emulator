namespace Chip8Emulator.Core
{
    internal interface IEmulatorShell
    {
        void DisassembleFile();
        void RunEmulator();
    }
}
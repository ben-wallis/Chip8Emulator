namespace Chip8Emulator.Core
{
    public interface IEmulatorShell
    {
        void DisassembleFile();
        void RunEmulator();
    }
}
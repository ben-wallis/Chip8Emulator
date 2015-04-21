using System;

namespace Chip8Emulator.Core
{
    public interface IEmulatorShell
    {
        IRegisterBank RegisterBank { get; }
        void DisassembleFile();
        void StartEmulation();
        void StopEmulation();
        event EventHandler EmulationStarted;
        event EventHandler EmulationStopped;
        event EventHandler DrawRequired;

        bool[,] DisplayPixels { get; }
    }
}
using System;

namespace Chip8Emulator.Core
{
    public interface IEmulatorShell
    {
        IRegisterBank RegisterBank { get; }
        bool[,] DisplayPixels { get; }

        event EventHandler EmulationStarted;
        event EventHandler EmulationStopped;
        event EventHandler DrawRequired;

        void DisassembleFile();
        void StartEmulation();
        void StopEmulation();
        void OnKeyDown(byte key);
        void OnKeyUp(byte key);
    }
}
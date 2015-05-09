using System;

namespace Chip8Emulator.Core
{
    public interface IEmulatorShell
    {
        event EventHandler EmulationStarted;
        event EventHandler EmulationStopped;
        event EventHandler CycleTick;
        event EventHandler DrawRequired;
        
        ushort CycleDelay { get; set; }
        bool[,] DisplayPixels { get; }
        void DisassembleFile();
        void StartEmulation(string romFilePath);
        void RunEmulationThread();
        void RunEmulator();
        void UpdateDisplay();
        void DecrementTimers();
        void OnKeyDown(byte key);
        void OnKeyUp(byte key);
        void StopEmulation();
    }
}
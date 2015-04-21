using System.Threading;
using Chip8Emulator.Core;

namespace Chip8Emulator.Interface
{
    internal class EmulatorShell : IEmulatorShell
    {
        private readonly IDisassembler _disassembler;
        private readonly ICpu _cpu;
        private readonly IFileHandler _fileHandler;
        private readonly IDisplay _display;

        public EmulatorShell(IDisassembler disassembler, ICpu cpu, IFileHandler fileHandler, IDisplay display)
        {
            _disassembler = disassembler;
            _cpu = cpu;
            _fileHandler = fileHandler;
            _display = display;
        }

        public void DisassembleFile()
        {
            _disassembler.DisassembleFile("c:\\chip8\\BREAKOUT");
        }

        public void RunEmulator()
        {
            _fileHandler.LoadFileIntoMemory("c:\\chip8\\CAR");
            while (true)
            {
                _cpu.EmulateOp();
                if (!_cpu.DrawRequired) continue;

                _display.DumpToConsole();
                Thread.Sleep(100);
            }
        }

    }
}

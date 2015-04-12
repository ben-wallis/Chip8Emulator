namespace Chip8Emulator
{
    internal class EmulatorShell : IEmulatorShell
    {
        private readonly IDisassembler _disassembler;

        public EmulatorShell(IDisassembler disassembler)
        {
            _disassembler = disassembler;
        }

        public void DisassembleFile()
        {
            _disassembler.DisassembleFile("c:\\chip8\\ANT");
        }
    }
}

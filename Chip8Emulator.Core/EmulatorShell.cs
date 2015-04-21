using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Chip8Emulator.Core
{
    public class EmulatorShell : IEmulatorShell
    {
        private readonly IDisassembler _disassembler;
        private readonly ICpu _cpu;
        private readonly IFileHandler _fileHandler;
        private readonly IDisplay _display;
        private readonly IRegisterBank _registerBank;

        public event EventHandler EmulationStarted;
        public event EventHandler EmulationStopped;
        public event EventHandler DrawRequired;

        internal bool RunEmulation;

        public EmulatorShell(IDisassembler disassembler, ICpu cpu, IFileHandler fileHandler, IDisplay display, IRegisterBank registerBank)
        {
            _disassembler = disassembler;
            _cpu = cpu;
            _fileHandler = fileHandler;
            _display = display;
            _registerBank = registerBank;
        }

        public bool[,] DisplayPixels
        {
            get { return _display.Pixels; }
        }

        public IRegisterBank RegisterBank
        {
            get { return _registerBank; }
        }

        public void DisassembleFile()
        {
            _disassembler.DisassembleFile("c:\\chip8\\Dragon2");
        }

        public virtual void RunEmulator()
        {
            while (RunEmulation)
            {
                RunEmulationCycle();
            }
            Console.WriteLine("Emulation Complete");
        }

        protected void RunEmulationCycle()
        {
            _cpu.EmulateOp();
            if (_cpu.DrawRequired)
            {
                if (DrawRequired != null) DrawRequired.Invoke(null, null);
            }
        }

        public void StartEmulation()
        {
            _registerBank.Initialise();
            _fileHandler.LoadFileIntoMemory("c:\\chip8\\HIDDEN");
            RunEmulation = true;
            Debug.WriteLine("Starting Emulation");
            if (EmulationStarted != null) EmulationStarted.Invoke(null, null);
           RunEmulationThread();
        }

        public virtual void RunEmulationThread()
        {
            //Task emulationTask = new Task(RunEmulator);
            //emulationTask.ContinueWith(EmulationExceptionHandler, TaskContinuationOptions.OnlyOnFaulted);
            //emulationTask.Start();
            //var emulationThread = new Thread(RunEmulator);
            //emulationThread.Start();

            var emulationThread = new Thread(() => SafeExecute(RunEmulator, EmulationExceptionHandler));
            emulationThread.Start();
        }

        private void SafeExecute(Action actionToExecute, Action<Exception> handler)
        {
            try
            {
                actionToExecute.Invoke();
            }
            catch (Exception ex)
            {
                EmulationExceptionHandler(ex);
            }
        }


        public void EmulationExceptionHandler(Exception exception)
        {
            throw exception.InnerException;
        }

        public void StopEmulation()
        {
            RunEmulation = false;
        }
    }
}

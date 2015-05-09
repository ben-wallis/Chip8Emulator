﻿using System;
using System.CodeDom;
using System.Diagnostics;
using System.Threading;

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

        public EventHandler<byte> KeyPressed;

        internal bool EmulationRunning;

        private long _cycleCount;

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

        public void StartEmulation(string romFilePath)
        {
            _registerBank.Initialise();
            _display.Initialise();

            _fileHandler.LoadFileIntoMemory(romFilePath);
            
            EmulationRunning = true;
            
            RunEmulationThread();
            RunDecrementTimersThread();
            RunUpdateDisplayThread();
            RunBenchmarkThread();

            if (EmulationStarted != null) EmulationStarted.Invoke(null, null);
        }

        public virtual void RunEmulationThread()
        {
            var emulationThread = new Thread(() => SafeExecute(RunEmulator));
            emulationThread.Start();
        }

        public virtual void RunEmulator()
        {
            Thread.CurrentThread.Name = "Run Emulator";
            Debug.WriteLine("Run Emulator Thread Started");

            _cycleCount = 0;
            while (EmulationRunning)
            {
                RunEmulationCycle();
                _cycleCount++;
                Thread.Sleep(10);
            }

            Console.WriteLine("Run Emulator Thread Exited");
        }

        protected void RunEmulationCycle()
        {
            _cpu.EmulateOp();
        }
        
        public void RunBenchmarkThread()
        {
            var benchmarkThread = new Thread(BenchmarkEmulation);
            benchmarkThread.Start();
        }

        public void BenchmarkEmulation()
        {
            Thread.CurrentThread.Name = "Benchmark Emulation";
            Console.WriteLine("Benchmark Emulation Thread Started");

            var lastCycleCount = _cycleCount;
            while (EmulationRunning)
            {
                var currentCycleCount = _cycleCount;
                var cyclesPerSecond = currentCycleCount - lastCycleCount;
                Console.WriteLine("Cycles per Second: {0}", cyclesPerSecond);
                lastCycleCount = currentCycleCount;
                Thread.Sleep(1000);
            }

            Console.WriteLine("Benchmark Emulation Thread Exited");
        }

        public void RunUpdateDisplayThread()
        {
            var updateDisplayThread = new Thread(UpdateDisplay);
            updateDisplayThread.Start();
        }

        public void UpdateDisplay()
        {
            try
            {
                Thread.CurrentThread.Name = "Update Display";
            }
            catch
            {
            }

            Console.WriteLine("Update Display Thread Started");
            
            while (EmulationRunning)
            {
                if (DrawRequired != null) DrawRequired.Invoke(null, null);
                Thread.Sleep(16);
            }
            
            Console.WriteLine("Update Display Thread Exited");
        }

        public void RunDecrementTimersThread()
        {
            var decrementTimersThread = new Thread(DecrementTimers);
            decrementTimersThread.Start();
        }

        public void DecrementTimers()
        {
            Thread.CurrentThread.Name = "Decrement Timers";
            Console.WriteLine("Decrement Timers Thread Started");

            while (EmulationRunning)
            {
                if (_registerBank.Delay > 0)
                {
                    _registerBank.Delay--;
                }

                if (_registerBank.Sound > 0)
                {
                    _registerBank.Sound--;
                }
                Thread.Sleep(16); // 60hz..ish
            }

            Console.WriteLine("Decrement Timers Thread Exited");
        }

        public void OnKeyDown(byte key)
        {
            _registerBank.Key = key;
            _registerBank.KeyPressed = true;
        }

        public void OnKeyUp(byte key)
        {
           _registerBank.KeyPressed = false;
        }

        private void SafeExecute(Action actionToExecute)
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
            EmulationRunning = false;
        }
    }
}

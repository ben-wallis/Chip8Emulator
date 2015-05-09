using Chip8Emulator.Core;

namespace Chip8Emulator.UI.ViewModels
{
    public class EmulatorRegistersViewModel
    {
        private readonly IRegisterBank _registerBank;

        public EmulatorRegistersViewModel(IRegisterBank registerBank)
        {
            _registerBank = registerBank;
        }
    }
}

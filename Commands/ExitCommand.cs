using HRSystem.Utils;
using System;

namespace HRSystem.Commands
{
    /// <summary>
    /// Команда для выхода из приложения.
    /// </summary>
    public class ExitCommand : ICommand
    {
        private readonly IConsoleHelper _consoleHelper;
        private readonly MenuManager _menuManager;

        public ExitCommand(IConsoleHelper consoleHelper)
        {
            _consoleHelper = consoleHelper;
            _menuManager = new MenuManager(consoleHelper);
        }

        public string Name => "Выход";

        public void Execute()
        {
            _menuManager.ClearScreen();
            _menuManager.PrintGoodbye();
        }
    }
}

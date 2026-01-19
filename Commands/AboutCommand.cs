using HRSystem.Utils;
using System;

namespace HRSystem.Commands
{
    /// <summary>
    /// Команда для отображения информации о разработчике.
    /// </summary>
    public class AboutCommand : ICommand
    {
        private readonly IConsoleHelper _consoleHelper;
        private readonly MenuManager _menuManager;

        public AboutCommand(IConsoleHelper consoleHelper)
        {
            _consoleHelper = consoleHelper;
            _menuManager = new MenuManager(consoleHelper);
        }

        public string Name => "О разработчике";

        public void Execute()
        {
            _menuManager.ClearScreen();
            _menuManager.PrintHeader("О разработчике");
            _menuManager.PrintInfo("Разработал kondrashov-m 2026");
            _menuManager.PrintInfo("Кондрашов Михаил Иванович mig2018kondrashov@gmail.com");
            _menuManager.WaitForKey();
        }
    }
}

using HRSystem.Utils;
using System;

namespace HRSystem.Commands
{
    /// <summary>
    /// Команда для управления настройками приложения.
    /// </summary>
    public class SettingsCommand : ICommand
    {
        private readonly IConsoleHelper _consoleHelper;
        private readonly MenuManager _menuManager;

        public SettingsCommand(IConsoleHelper consoleHelper)
        {
            _consoleHelper = consoleHelper;
            _menuManager = new MenuManager(consoleHelper);
        }

        public string Name => "Настройки";

        public void Execute()
        {
            _menuManager.ClearScreen();
            _menuManager.PrintHeader("НАСТРОЙКИ");
            _menuManager.PrintInfo($"Текущая валюта: {AppSettings.Currency}");
            _menuManager.PrintInfo("1. Установить RUB");
            _menuManager.PrintInfo("2. Установить USD");
            var choice = _menuManager.GetInput("Выберите");
            
            if (choice == "1")
                AppSettings.Currency = Currency.RUB;
            else if (choice == "2")
                AppSettings.Currency = Currency.USD;
            
            _menuManager.WaitForKey();
        }
    }
}

using System;
using System.Collections.Generic;

namespace HRSystem.Utils
{
    /// <summary>
    /// Менеджер для управления пользовательским интерфейсом меню.
    /// </summary>
    public class MenuManager
    {
        private readonly IConsoleHelper _consoleHelper;

        public MenuManager(IConsoleHelper consoleHelper)
        {
            _consoleHelper = consoleHelper;
        }

        /// <summary>
        /// Выводит красивый заголовок.
        /// </summary>
        public void PrintHeader(string title)
        {
            _consoleHelper.WriteLine("");
            _consoleHelper.WriteLine("╔════════════════════════════════════════════════════════════╗");
            _consoleHelper.WriteLine($"║  {CenterText(title, 56)}  ║");
            _consoleHelper.WriteLine("╚════════════════════════════════════════════════════════════╝");
            _consoleHelper.WriteLine("");
        }

        /// <summary>
        /// Выводит разделитель.
        /// </summary>
        public void PrintSeparator()
        {
            _consoleHelper.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
        }

        /// <summary>
        /// Выводит меню с опциями.
        /// </summary>
        public void PrintMenu(List<MenuItem> items)
        {
            foreach (var item in items)
            {
                _consoleHelper.WriteLine($"  ► {item.Number}. {item.Title,-50}");
            }
            _consoleHelper.WriteLine("");
        }

        /// <summary>
        /// Выводит сообщение об ошибке.
        /// </summary>
        public void PrintError(string message)
        {
            _consoleHelper.WriteLine($"  ✗ ОШИБКА: {message}");
            _consoleHelper.WriteLine("");
        }

        /// <summary>
        /// Выводит сообщение об успехе.
        /// </summary>
        public void PrintSuccess(string message)
        {
            _consoleHelper.WriteLine($"  ✓ УСПЕХ: {message}");
            _consoleHelper.WriteLine("");
        }

        /// <summary>
        /// Выводит информационное сообщение.
        /// </summary>
        public void PrintInfo(string message)
        {
            _consoleHelper.WriteLine($"  ℹ {message}");
        }

        /// <summary>
        /// Выводит приглашение к вводу.
        /// </summary>
        public string GetInput(string prompt)
        {
            _consoleHelper.Write($"  ➤ {prompt}: ");
            return _consoleHelper.ReadLine();
        }

        /// <summary>
        /// Выводит сообщение о выходе.
        /// </summary>
        public void PrintGoodbye()
        {
            _consoleHelper.WriteLine("");
            _consoleHelper.WriteLine("╔════════════════════════════════════════════════════════════╗");
            _consoleHelper.WriteLine($"║  {CenterText("До свидания!", 56)}  ║");
            _consoleHelper.WriteLine("╚════════════════════════════════════════════════════════════╝");
            _consoleHelper.WriteLine("");
        }

        /// <summary>
        /// Центрирует текст по длине.
        /// </summary>
        private string CenterText(string text, int width)
        {
            if (text.Length >= width)
                return text;

            int totalPadding = width - text.Length;
            int leftPadding = totalPadding / 2;
            int rightPadding = totalPadding - leftPadding;

            return new string(' ', leftPadding) + text + new string(' ', rightPadding);
        }

        /// <summary>
        /// Очищает экран с задержкой.
        /// </summary>
        public void ClearScreen()
        {
            _consoleHelper.ClearScreen();
        }

        /// <summary>
        /// Ждёт нажатия клавиши.
        /// </summary>
        public void WaitForKey(string message = "Нажмите любую клавишу для продолжения...")
        {
            _consoleHelper.WriteLine($"  {message}");
            Console.ReadKey(true);
        }
    }

    /// <summary>
    /// Элемент меню.
    /// </summary>
    public class MenuItem
    {
        public string Number { get; set; }
        public string Title { get; set; }

        public MenuItem(string number, string title)
        {
            Number = number;
            Title = title;
        }
    }
}

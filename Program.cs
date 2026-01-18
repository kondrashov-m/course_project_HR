using System;
using HRSystem.Services;
using System.Runtime.InteropServices;
using System.Text;
using HRSystem.Repositories;
using HRSystem.Utils;
using HRSystem.Commands;

namespace HRSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            // Ensure console can handle code pages and configure sensible defaults for Windows
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            // Prefer UTF-8 for both output and input. On Windows also set console code page to 65001 (UTF-8).
            var utf8 = new UTF8Encoding(false);
            Console.OutputEncoding = utf8;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                // Try to set the Windows console code page to UTF-8 (65001)
                try
                {
                    NativeMethods.SetConsoleOutputCP(65001);
                    NativeMethods.SetConsoleCP(65001);
                }
                catch { }

                Console.InputEncoding = utf8;
            }
            else
            {
                Console.InputEncoding = utf8;
            }
            // Создаем репозиторий и сервисы
            var repository = new InMemoryRepository();
            var employeeService = new EmployeeService(repository);
            var vacationService = new VacationService();
            var salaryService = new SalaryService();
            var departmentService = new DepartmentServiceImpl();
            var consoleHelper = new ConsoleHelper();
            var inputValidator = new InputValidator(consoleHelper);
            var menuManager = new MenuManager(consoleHelper);

            // Создаем команды
            var employeeCommand = new EmployeeCommand(employeeService, consoleHelper, inputValidator);
            var vacationCommand = new VacationCommand(vacationService, employeeService, consoleHelper);
            var salaryCommand = new SalaryCommand(salaryService, employeeService, consoleHelper);
            var departmentCommand = new DepartmentCommand(departmentService, consoleHelper);

            // Запускаем основной цикл
            while (true)
            {
                menuManager.ClearScreen();
                menuManager.PrintHeader("HR System");
                
                var menuItems = new System.Collections.Generic.List<MenuItem>
                {
                    new MenuItem("1", "Управление сотрудниками"),
                    new MenuItem("2", "Управление отпусками"),
                    new MenuItem("3", "Управление зарплатой"),
                    new MenuItem("4", "Управление отделами"),
                    new MenuItem("5", "Настройки"),
                    new MenuItem("6", "О разработчике"),
                    new MenuItem("7", "Выход")
                };

                menuManager.PrintMenu(menuItems);
                
                var choice = menuManager.GetInput("Выберите пункт меню");
                
                switch (choice)
                {
                    case "1":
                        menuManager.ClearScreen();
                        menuManager.PrintHeader("УПРАВЛЕНИЕ СОТРУДНИКАМИ");
                        employeeCommand.Execute();
                        menuManager.WaitForKey();
                        break;
                    case "2":
                        menuManager.ClearScreen();
                        menuManager.PrintHeader("УПРАВЛЕНИЕ ОТПУСКАМИ");
                        vacationCommand.Execute();
                        menuManager.WaitForKey();
                        break;
                    case "3":
                        menuManager.ClearScreen();
                        menuManager.PrintHeader("УПРАВЛЕНИЕ ЗАРПЛАТОЙ");
                        salaryCommand.Execute();
                        menuManager.WaitForKey();
                        break;
                    case "4":
                        menuManager.ClearScreen();
                        menuManager.PrintHeader("УПРАВЛЕНИЕ ОТДЕЛАМИ");
                        departmentCommand.Execute();
                        menuManager.WaitForKey();
                        break;
                    case "5":
                        menuManager.ClearScreen();
                        menuManager.PrintHeader("НАСТРОЙКИ");
                        menuManager.PrintInfo($"Текущая валюта: {HRSystem.Utils.AppSettings.Currency}");
                        menuManager.PrintInfo("1. Установить RUB");
                        menuManager.PrintInfo("2. Установить USD");
                        var sc = menuManager.GetInput("Выберите");
                        if (sc == "1") HRSystem.Utils.AppSettings.Currency = HRSystem.Utils.Currency.RUB;
                        else if (sc == "2") HRSystem.Utils.AppSettings.Currency = HRSystem.Utils.Currency.USD;
                        menuManager.WaitForKey();
                        break;
                    case "6":
                        menuManager.ClearScreen();
                        menuManager.PrintHeader("О разработчике");
                        menuManager.PrintInfo("Разработал kondrashov-m 2026");
                        menuManager.PrintInfo("Кондрашов Михаил Иванович mig2018kondrashov@gmail.com");
                        menuManager.WaitForKey();
                        break;
                    // duplicate case removed (handled above as Настройки)
                    case "7":
                        menuManager.ClearScreen();
                        menuManager.PrintGoodbye();
                        return;
                    default:
                        menuManager.PrintError("Неверный выбор. Попробуйте снова.");
                        menuManager.WaitForKey();
                        break;
                }
            }
        }
    }

    internal static class NativeMethods
    {
        [System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool SetConsoleOutputCP(uint wCodePageID);

        [System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool SetConsoleCP(uint wCodePageID);
    }
}

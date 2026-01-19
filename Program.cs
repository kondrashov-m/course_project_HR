using System;
using HRSystem.Services;
using System.Runtime.InteropServices;
using System.Text;
using System.IO;
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
            var vacationService = new VacationService(repository);
            var salaryService = new SalaryService(repository);
            var departmentService = new DepartmentServiceImpl();
            var consoleHelper = new ConsoleHelper();
            var inputValidator = new InputValidator(consoleHelper);
            var menuManager = new MenuManager(consoleHelper);

            // Создаем команды
            var employeeCommand = new EmployeeCommand(employeeService, departmentService, consoleHelper, inputValidator);
            var vacationCommand = new VacationCommand(vacationService, employeeService, consoleHelper);
            var salaryCommand = new SalaryCommand(salaryService, employeeService, consoleHelper);
            var departmentCommand = new DepartmentCommand(departmentService, consoleHelper);
            var settingsCommand = new SettingsCommand(consoleHelper);
            var aboutCommand = new AboutCommand(consoleHelper);
            var exitCommand = new ExitCommand(consoleHelper);

            // Словарь команд для маршрутизации
            var commands = new System.Collections.Generic.Dictionary<string, ICommand>
            {
                { "1", employeeCommand },
                { "2", vacationCommand },
                { "3", salaryCommand },
                { "4", departmentCommand },
                { "5", settingsCommand },
                { "6", aboutCommand },
                { "7", exitCommand }
            };

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
                
                if (commands.TryGetValue(choice, out var command))
                {
                    if (choice == "7") // Для команды выхода
                    {
                        command.Execute();
                        return;
                    }
                    else
                    {
                        menuManager.ClearScreen();
                        menuManager.PrintHeader($"Выбран пункт: {command.Name}".ToUpper());
                        command.Execute();
                        menuManager.WaitForKey();
                    }
                }
                else
                {
                    menuManager.PrintError("Неверный выбор. Попробуйте снова.");
                    menuManager.WaitForKey();
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

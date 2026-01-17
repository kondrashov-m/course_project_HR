using System;
using HRSystem.Services;
using HRSystem.Repositories;
using HRSystem.Utils;
using HRSystem.Commands;

namespace HRSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            // Создаем репозиторий и сервис
            var repository = new InMemoryRepository();
            var employeeService = new EmployeeService(repository);
            var consoleHelper = new ConsoleHelper();

            // Создаем команду
            var command = new EmployeeCommand(employeeService, consoleHelper);

            // Запускаем основной цикл
            while (true)
            {
                consoleHelper.WriteLine("=== Меню ===");
                consoleHelper.WriteLine("1. Сотрудники");
                consoleHelper.WriteLine("2. Выход");

                var choice = consoleHelper.ReadLine();
                if (choice == "2")
                {
                    consoleHelper.WriteLine("До свидания!");
                    break;
                }
                else if (choice == "1")
                {
                    command.Execute();
                }
                else
                {
                    consoleHelper.WriteLine("Неверный выбор. Попробуйте снова.");
                }
            }
        }
    }
}

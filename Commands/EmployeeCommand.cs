using HRSystem.Models;
using HRSystem.Services;
using HRSystem.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HRSystem.Commands
{
    /// <summary>
    /// Команда для управления сотрудниками.
    /// </summary>
    public class EmployeeCommand : ICommand
    {
        private readonly IEmployeeService _employeeService;
        private readonly IConsoleHelper _consoleHelper;

        public EmployeeCommand(IEmployeeService employeeService, IConsoleHelper consoleHelper)
        {
            _employeeService = employeeService;
            _consoleHelper = consoleHelper;
        }

        public string Name => "Сотрудники";

        public void Execute()
        {
            _consoleHelper.WriteLine("=== Управление сотрудниками ===");
            Console.WriteLine("1. Добавить сотрудника");
            Console.WriteLine("2. Просмотреть всех сотрудников");
            Console.WriteLine("3. Просмотреть по отделу");
            Console.WriteLine("4. Просмотреть по должности");
            Console.WriteLine("5. Обновить сотрудника");
            Console.WriteLine("6. Удалить сотрудника");
            Console.WriteLine("7. Назад");

            var choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    AddEmployee();
                    break;
                case "2":
                    ViewAllEmployees();
                    break;
                case "3":
                    ViewByDepartment();
                    break;
                case "4":
                    ViewByPosition();
                    break;
                case "5":
                    UpdateEmployee();
                    break;
                case "6":
                    DeleteEmployee();
                    break;
                case "7":
                    return;
                default:
                    Console.WriteLine("Неверный выбор.");
                    break;
            }
        }

        private void AddEmployee()
        {
            _consoleHelper.WriteLine("=== Добавление сотрудника ===");

            var firstName = InputValidator.ValidateString("Имя: ");
            var lastName = InputValidator.ValidateString("Фамилия: ");
            var positionName = InputValidator.ValidateString("Должность: ");
            var departmentName = InputValidator.ValidateString("Отдел: ");
            var baseSalary = InputValidator.ValidateDecimal("Зарплата: ");

            var employee = new Employee(firstName, lastName, positionName, departmentName, baseSalary);
            _employeeService.AddEmployee(employee);
            _consoleHelper.WriteLine("Сотрудник добавлен успешно!");
        }

        private void ViewAllEmployees()
        {
            var employees = _employeeService.GetAllEmployees();
            if (employees.Count == 0)
            {
                _consoleHelper.WriteLine("Список сотрудников пуст.");
                return;
            }

            foreach (var emp in employees)
            {
                _consoleHelper.WriteLine(emp.ToString());
            }
        }

        private void ViewByDepartment()
        {
            var departments = _employeeService.GetAllEmployees()
                .Select(e => e.DepartmentName)
                .Distinct()
                .ToList();

            _consoleHelper.WriteLine("=== Отделы ===");
            foreach (var dept in departments)
            {
                _consoleHelper.WriteLine(dept);
            }

            Console.Write("Выберите отдел: ");
            var dept = Console.ReadLine();
            var employees = _employeeService.GetEmployeesByDepartment(dept);
            if (employees.Count == 0)
            {
                _consoleHelper.WriteLine("Сотрудников в этом отделе нет.");
            }
            else
            {
                foreach (var emp in employees)
                {
                    _consoleHelper.WriteLine(emp.ToString());
                }
            }
        }

        private void ViewByPosition()
        {
            var positions = _employeeService.GetAllEmployees()
                .Select(e => e.PositionName)
                .Distinct()
                .ToList();

            _consoleHelper.WriteLine("=== Должности ===");
            foreach (var pos in positions)
            {
                _consoleHelper.WriteLine(pos);
            }

            Console.Write("Выберите должность: ");
            var pos = Console.ReadLine();
            var employees = _employeeService.GetEmployeesByPosition(pos);
            if (employees.Count == 0)
            {
                _consoleHelper.WriteLine("Сотрудников с этой должностью нет.");
            }
            else
            {
                foreach (var emp in employees)
                {
                    _consoleHelper.WriteLine(emp.ToString());
                }
            }
        }

        private void UpdateEmployee()
        {
            Console.Write("Введите ID сотрудника для обновления: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var emp = _employeeService.GetEmployeeById(id);
                if (emp == null)
                {
                    _consoleHelper.WriteLine("Сотрудник не найден.");
                    return;
                }

                Console.Write("Новое имя: ");
                emp.FirstName = Console.ReadLine();
                Console.Write("Новая фамилия: ");
                emp.LastName = Console.ReadLine();
                Console.Write("Новая должность: ");
                emp.PositionName = Console.ReadLine();
                Console.Write("Новый отдел: ");
                emp.DepartmentName = Console.ReadLine();
                Console.Write("Новая зарплата: ");
                emp.BaseSalary = InputValidator.ValidateDecimal("Зарплата: ");

                _employeeService.UpdateEmployee(emp);
                _consoleHelper.WriteLine("Сотрудник обновлен!");
            }
        }

        private void DeleteEmployee()
        {
            Console.Write("Введите ID сотрудника для удаления: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                _employeeService.DeleteEmployee(id);
                _consoleHelper.WriteLine("Сотрудник удален!");
            }
        }
    }
}

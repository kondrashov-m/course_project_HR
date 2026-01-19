using HRSystem.Models;
using HRSystem.Services;
using HRSystem.Utils;
using System;
using System.Collections.Generic;

namespace HRSystem.Commands
{
    /// <summary>
    /// Команда для управления сотрудниками с расширенным функционалом.
    /// </summary>
    public class EmployeeCommand : ICommand
    {
        private readonly IEmployeeService _employeeService;
        private readonly IDepartmentService _departmentService;
        private readonly IConsoleHelper _consoleHelper;
        private readonly IInputValidator _inputValidator;
        private readonly MenuManager _menuManager;

        public EmployeeCommand(IEmployeeService employeeService, IDepartmentService departmentService, IConsoleHelper consoleHelper, IInputValidator inputValidator)
        {
            _employeeService = employeeService;
            _departmentService = departmentService;
            _consoleHelper = consoleHelper;
            _inputValidator = inputValidator;
            _menuManager = new MenuManager(consoleHelper);
        }

        public string Name => "Сотрудники";

        public void Execute()
        {
            while (true)
            {
                var menuItems = new List<MenuItem>
                {
                    new MenuItem("1", "Добавить сотрудника"),
                    new MenuItem("2", "Просмотреть всех сотрудников"),
                    new MenuItem("3", "Найти сотрудника по ID"),
                    new MenuItem("4", "Поиск по отделу"),
                    new MenuItem("5", "Поиск по должности"),
                    new MenuItem("6", "Обновить сотрудника"),
                    new MenuItem("7", "Удалить сотрудника"),
                    new MenuItem("8", "Назад")
                };

                _menuManager.PrintMenu(menuItems);
                
                var choice = _menuManager.GetInput("Выберите действие");

                switch (choice)
                {
                    case "1":
                        AddEmployee();
                        break;
                    case "2":
                        ViewAllEmployees();
                        break;
                    case "3":
                        ViewByEmployeeId();
                        break;
                    case "4":
                        ViewByDepartment();
                        break;
                    case "5":
                        ViewByPosition();
                        break;
                    case "6":
                        UpdateEmployee();
                        break;
                    case "7":
                        DeleteEmployee();
                        break;
                    case "8":
                        return;
                    default:
                        _menuManager.PrintError("Неверный выбор");
                        break;
                }
            }
        }

        private void AddEmployee()
        {
            _menuManager.PrintHeader("Добавление сотрудника");

            _menuManager.PrintInfo("Существующие отделы и номера:");
            var allDepartments = _departmentService.GetAllDepartments();
            var departments = new List<string>();
            var positions = new List<string>();
            
            // Получаем названия отделов из сервиса
            foreach (var d in allDepartments)
            {
                if (!string.IsNullOrWhiteSpace(d.Name))
                    departments.Add(d.Name);
            }
            
            // Получаем должности из сотрудников
            var existing = _employeeService.GetAllEmployees();
            foreach (var e in existing)
            {
                if (!string.IsNullOrWhiteSpace(e.PositionName) && !positions.Contains(e.PositionName)) 
                    positions.Add(e.PositionName);
            }

            for (int i = 0; i < departments.Count; i++)
                _menuManager.PrintInfo($"  {i + 1}. {departments[i]}");
            _menuManager.PrintInfo("  0. Ввести новый отдел");
            var deptChoice = _menuManager.GetInput("Выберите отдел (номер) или 0");
            string departmentName;
            if (int.TryParse(deptChoice, out int dch) && dch > 0 && dch <= departments.Count)
                departmentName = departments[dch - 1];
            else
                departmentName = _inputValidator.ValidateString("Отдел");

            for (int i = 0; i < positions.Count; i++)
                _menuManager.PrintInfo($"  {i + 1}. {positions[i]}");
            _menuManager.PrintInfo("  0. Ввести новую должность");
            var posChoice = _menuManager.GetInput("Выберите должность (номер) или 0");
            string positionName;
            if (int.TryParse(posChoice, out int pch) && pch > 0 && pch <= positions.Count)
                positionName = positions[pch - 1];
            else
                positionName = _inputValidator.ValidateString("Должность");

            var firstName = _inputValidator.ValidateString("Имя");
            var lastName = _inputValidator.ValidateString("Фамилия");
            var baseSalary = _inputValidator.ValidateDecimal("Базовая зарплата");

            var employee = new Employee(firstName, lastName, positionName, departmentName, baseSalary);
            _employeeService.AddEmployee(employee);
            _menuManager.PrintInfo($"DEBUG: First='{employee.FirstName}' Last='{employee.LastName}' Pos='{employee.PositionName}' Dept='{employee.DepartmentName}'");
            _menuManager.PrintSuccess($"Сотрудник с ID {employee.Id} успешно добавлен");
            _menuManager.WaitForKey();
        }

        private void ViewAllEmployees()
        {
            _menuManager.PrintHeader("Все сотрудники");

            var employees = _employeeService.GetAllEmployees();
            if (employees.Count == 0)
            {
                _menuManager.PrintInfo("Список сотрудников пуст");
                _menuManager.WaitForKey();
                return;
            }

            PrintEmployeesList(employees);
            _menuManager.WaitForKey();
        }

        private void ViewByEmployeeId()
        {
            _menuManager.PrintHeader("Поиск сотрудника по ID");

            var id = _inputValidator.ValidateInteger("Введите ID сотрудника");

            var employee = _employeeService.GetEmployeeById(id);
            if (employee == null)
            {
                _menuManager.PrintError($"Сотрудник с ID {id} не найден");
            }
            else
            {
                _menuManager.PrintInfo($"ID: {employee.Id} | {employee.FirstName} {employee.LastName} | " +
                    $"{employee.PositionName} | {employee.DepartmentName} | Зарплата: {employee.BaseSalary:F2}");
            }
            
            _menuManager.WaitForKey();
        }

        private void ViewByDepartment()
        {
            _menuManager.PrintHeader("Поиск по отделу");

            var departments = _employeeService.GetAllEmployees();
            if (departments.Count == 0)
            {
                _menuManager.PrintInfo("Нет сотрудников в системе");
                _menuManager.WaitForKey();
                return;
            }

            var deptName = _menuManager.GetInput("Название отдела");
            while (string.IsNullOrWhiteSpace(deptName))
            {
                _menuManager.PrintError("Название отдела не может быть пустым");
                deptName = _menuManager.GetInput("Название отдела");
            }
            var employees = _employeeService.GetEmployeesByDepartment(deptName);

            if (employees.Count == 0)
            {
                _menuManager.PrintError($"Сотрудников в отделе '{deptName}' не найдено");
            }
            else
            {
                _menuManager.PrintInfo($"Найдено {employees.Count} сотрудников в отделе '{deptName}'");
                PrintEmployeesList(employees);
            }
            
            _menuManager.WaitForKey();
        }

        private void ViewByPosition()
        {
            _menuManager.PrintHeader("Поиск по должности");

            var posName = _menuManager.GetInput("Название должности");
            while (string.IsNullOrWhiteSpace(posName))
            {
                _menuManager.PrintError("Название должности не может быть пустым");
                posName = _menuManager.GetInput("Название должности");
            }
            var employees = _employeeService.GetEmployeesByPosition(posName);

            if (employees.Count == 0)
            {
                _menuManager.PrintError($"Сотрудников на должности '{posName}' не найдено");
            }
            else
            {
                _menuManager.PrintInfo($"Найдено {employees.Count} сотрудников на должности '{posName}'");
                PrintEmployeesList(employees);
            }
            
            _menuManager.WaitForKey();
        }

        private void UpdateEmployee()
        {
            _menuManager.PrintHeader("Обновление сотрудника");

            var id = _inputValidator.ValidateInteger("Введите ID сотрудника для обновления");

            var emp = _employeeService.GetEmployeeById(id);
            if (emp == null)
            {
                _menuManager.PrintError($"Сотрудник с ID {id} не найден");
                _menuManager.WaitForKey();
                return;
            }

            _menuManager.PrintInfo($"Обновление: {emp.FirstName} {emp.LastName} (ID: {id})");
            
            _consoleHelper.Write($"  ➤ Новое имя (текущее: {emp.FirstName}): ");
            var newFirstName = _consoleHelper.ReadLine();
            if (!string.IsNullOrEmpty(newFirstName)) emp.FirstName = newFirstName;

            _consoleHelper.Write($"  ➤ Новая фамилия (текущее: {emp.LastName}): ");
            var newLastName = _consoleHelper.ReadLine();
            if (!string.IsNullOrEmpty(newLastName)) emp.LastName = newLastName;

            _consoleHelper.Write($"  ➤ Новая должность (текущее: {emp.PositionName}): ");
            var newPos = _consoleHelper.ReadLine();
            if (!string.IsNullOrEmpty(newPos)) emp.PositionName = newPos;

            _consoleHelper.Write($"  ➤ Новый отдел (текущее: {emp.DepartmentName}): ");
            var newDept = _consoleHelper.ReadLine();
            if (!string.IsNullOrEmpty(newDept)) emp.DepartmentName = newDept;

            _consoleHelper.Write($"  ➤ Новая зарплата (текущее: {emp.BaseSalary:F2}): ");
            if (decimal.TryParse(_consoleHelper.ReadLine(), out decimal newSalary))
                emp.BaseSalary = newSalary;

            _employeeService.UpdateEmployee(emp);
            _menuManager.PrintSuccess($"Сотрудник с ID {id} успешно обновлен");
            _menuManager.WaitForKey();
        }

        private void DeleteEmployee()
        {
            _menuManager.PrintHeader("Удаление сотрудника");

            var id = _inputValidator.ValidateInteger("Введите ID сотрудника для удаления");

            var emp = _employeeService.GetEmployeeById(id);
            if (emp == null)
            {
                _menuManager.PrintError($"Сотрудник с ID {id} не найден");
                _menuManager.WaitForKey();
                return;
            }

            _consoleHelper.WriteLine($"  ⚠ Вы уверены, что хотите удалить сотрудника: {emp.FirstName} {emp.LastName} (ID: {id})? (y/n)");
            var confirm = _consoleHelper.ReadLine()?.ToLower();
            
            if (confirm == "y" || confirm == "yes")
            {
                _employeeService.DeleteEmployee(id);
                _menuManager.PrintSuccess($"Сотрудник с ID {id} успешно удален");
            }
            else
            {
                _menuManager.PrintInfo("Удаление отменено");
            }
            
            _menuManager.WaitForKey();
        }

        private void PrintEmployeesList(List<Employee> employees)
        {
            _menuManager.PrintSeparator();
            foreach (var emp in employees)
            {
                var name = $"{emp.FirstName} {emp.LastName}".Trim();
                if (string.IsNullOrWhiteSpace(name))
                    name = !string.IsNullOrWhiteSpace(emp.PositionName) ? emp.PositionName : (!string.IsNullOrWhiteSpace(emp.DepartmentName) ? emp.DepartmentName : "(нет имени)");

                _consoleHelper.WriteLine($"  ID: {emp.Id:D3} | {name,-30} | {emp.PositionName,-20} | {emp.DepartmentName,-15} | Зарплата: {HRSystem.Utils.AppSettings.FormatMoney(emp.BaseSalary)}");
            }
            _menuManager.PrintSeparator();
        }
    }
}

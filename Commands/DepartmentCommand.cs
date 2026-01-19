using HRSystem.Models;
using HRSystem.Services;
using HRSystem.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HRSystem.Commands
{
    /// <summary>
    /// Команда для управления отделами.
    /// </summary>
    public class DepartmentCommand : ICommand
    {
        private readonly IDepartmentService _departmentService;
        private readonly IConsoleHelper _consoleHelper;
        private readonly MenuManager _menuManager;

        public DepartmentCommand(IDepartmentService departmentService, IConsoleHelper consoleHelper)
        {
            _departmentService = departmentService;
            _consoleHelper = consoleHelper;
            _menuManager = new MenuManager(consoleHelper);
        }

        public string Name => "Отделы";

        public void Execute()
        {
            while (true)
            {
                _menuManager.ClearScreen();
                _menuManager.PrintHeader("УПРАВЛЕНИЕ ОТДЕЛАМИ");
                var menuItems = new System.Collections.Generic.List<MenuItem>
                {
                    new MenuItem("1", "Добавить отдел"),
                    new MenuItem("2", "Просмотреть все отделы"),
                    new MenuItem("3", "Обновить отдел"),
                    new MenuItem("4", "Удалить отдел"),
                    new MenuItem("5", "Назад")
                };

                _menuManager.PrintMenu(menuItems);
                var choice = _menuManager.GetInput("Выберите действие");

                switch (choice)
                {
                    case "1":
                        AddDepartment();
                        break;
                    case "2":
                        ViewAllDepartments();
                        break;
                    case "3":
                        UpdateDepartment();
                        break;
                    case "4":
                        DeleteDepartment();
                        break;
                    case "5":
                        return;
                    default:
                        _menuManager.PrintError("Неверный выбор.");
                        _menuManager.WaitForKey();
                        break;
                }
            }
        }

        private void AddDepartment()
        {
            _menuManager.PrintHeader("Добавление отдела");
            var name = _consoleHelper.ReadLine(); // Используем _consoleHelper вместо Console.ReadLine()
            if (string.IsNullOrWhiteSpace(name))
            {
                _menuManager.PrintError("Название отдела не может быть пустым!");
                _menuManager.WaitForKey();
                return;
            }

            _consoleHelper.Write("  ➤ Описание отдела: ");
            var description = _consoleHelper.ReadLine();

            var department = new Department
            {
                Name = name,
                Description = description
            };

            _departmentService.AddDepartment(department);
            _menuManager.PrintSuccess("Отдел успешно добавлен!");
            _menuManager.WaitForKey();
        }

        private void ViewAllDepartments()
        {
            _menuManager.PrintHeader("Все отделы");
            var departments = _departmentService.GetAllDepartments();
            if (departments.Count == 0)
            {
                _menuManager.PrintInfo("Нет отделов в системе.");
                _menuManager.WaitForKey();
                return;
            }

            foreach (var dept in departments)
            {
                _menuManager.PrintInfo($"ID: {dept.Id}, Название: {dept.Name}, Описание: {dept.Description}");
            }
            _menuManager.WaitForKey();
        }

        private void UpdateDepartment()
        {
            _menuManager.PrintHeader("Обновление отдела");
            _consoleHelper.Write("  ➤ ID отдела для обновления: ");
            if (int.TryParse(_consoleHelper.ReadLine(), out int deptId))
            {
                var dept = _departmentService.GetDepartmentById(deptId);
                if (dept == null)
                {
                    _menuManager.PrintError("Отдел не найден.");
                    _menuManager.WaitForKey();
                    return;
                }

                _consoleHelper.Write($"  ➤ Новое название (текущее: {dept.Name}): ");
                var newName = _consoleHelper.ReadLine();
                if (!string.IsNullOrEmpty(newName))
                    dept.Name = newName;

                _consoleHelper.Write($"  ➤ Новое описание (текущее: {dept.Description}): ");
                var newDesc = _consoleHelper.ReadLine();
                if (!string.IsNullOrEmpty(newDesc))
                    dept.Description = newDesc;

                _departmentService.UpdateDepartment(dept);
                _menuManager.PrintSuccess("Отдел обновлен!");
                _menuManager.WaitForKey();
            }
            else
            {
                _menuManager.PrintError("Неверный ID.");
                _menuManager.WaitForKey();
            }
        }

        private void DeleteDepartment()
        {
            _menuManager.PrintHeader("Удаление отдела");
            _consoleHelper.Write("  ➤ ID отдела для удаления: ");
            if (int.TryParse(_consoleHelper.ReadLine(), out int deptId))
            {
                _departmentService.DeleteDepartment(deptId);
                _menuManager.PrintSuccess("Отдел удален!");
                _menuManager.WaitForKey();
            }
            else
            {
                _menuManager.PrintError("Неверный ID.");
                _menuManager.WaitForKey();
            }
        }
    }
}

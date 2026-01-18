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

        public DepartmentCommand(IDepartmentService departmentService, IConsoleHelper consoleHelper)
        {
            _departmentService = departmentService;
            _consoleHelper = consoleHelper;
        }

        public string Name => "Отделы";

        public void Execute()
        {
            Console.WriteLine("1. Добавить отдел");
            Console.WriteLine("2. Просмотреть все отделы");
            Console.WriteLine("3. Обновить отдел");
            Console.WriteLine("4. Удалить отдел");
            Console.WriteLine("5. Назад");

            var choice = Console.ReadLine();
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
                    Console.WriteLine("Неверный выбор.");
                    break;
            }
        }

        private void AddDepartment()
        {
            Console.Write("Название отдела: ");
            var name = Console.ReadLine();

            Console.Write("Описание отдела: ");
            var description = Console.ReadLine();

            var department = new Department
            {
                Name = name,
                Description = description
            };

            _departmentService.AddDepartment(department);
            _consoleHelper.WriteLine("Отдел успешно добавлен!");
        }

        private void ViewAllDepartments()
        {
            var departments = _departmentService.GetAllDepartments();
            if (departments.Count == 0)
            {
                _consoleHelper.WriteLine("Нет отделов в системе.");
                return;
            }

            foreach (var dept in departments)
            {
                _consoleHelper.WriteLine($"ID: {dept.Id}, Название: {dept.Name}, " +
                    $"Описание: {dept.Description}");
            }
        }

        private void UpdateDepartment()
        {
            Console.Write("ID отдела для обновления: ");
            if (int.TryParse(Console.ReadLine(), out int deptId))
            {
                var dept = _departmentService.GetDepartmentById(deptId);
                if (dept == null)
                {
                    _consoleHelper.WriteLine("Отдел не найден.");
                    return;
                }

                Console.Write("Новое название (текущее: {0}): ", dept.Name);
                var newName = Console.ReadLine();
                if (!string.IsNullOrEmpty(newName))
                    dept.Name = newName;

                Console.Write("Новое описание (текущее: {0}): ", dept.Description);
                var newDesc = Console.ReadLine();
                if (!string.IsNullOrEmpty(newDesc))
                    dept.Description = newDesc;

                _departmentService.UpdateDepartment(dept);
                _consoleHelper.WriteLine("Отдел обновлен!");
            }
        }

        private void DeleteDepartment()
        {
            Console.Write("ID отдела для удаления: ");
            if (int.TryParse(Console.ReadLine(), out int deptId))
            {
                _departmentService.DeleteDepartment(deptId);
                _consoleHelper.WriteLine("Отдел удален!");
            }
        }
    }
}

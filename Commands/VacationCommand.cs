using HRSystem.Models;
using HRSystem.Services;
using HRSystem.Utils;
using System;
using System.Collections.Generic;

namespace HRSystem.Commands
{
    /// <summary>
    /// Команда для управления отпусками с ID-привязкой.
    /// </summary>
    public class VacationCommand : ICommand
    {
        private readonly IVacationService _vacationService;
        private readonly IEmployeeService _employeeService;
        private readonly IConsoleHelper _consoleHelper;
        private readonly MenuManager _menuManager;

        public VacationCommand(IVacationService vacationService, IEmployeeService employeeService, IConsoleHelper consoleHelper)
        {
            _vacationService = vacationService;
            _employeeService = employeeService;
            _consoleHelper = consoleHelper;
            _menuManager = new MenuManager(consoleHelper);
        }

        public string Name => "Отпуска";

        public void Execute()
        {
            while (true)
            {
                var menuItems = new List<MenuItem>
                {
                    new MenuItem("1", "Добавить отпуск"),
                    new MenuItem("2", "Просмотреть все отпуска"),
                    new MenuItem("3", "Найти отпуск по ID"),
                    new MenuItem("4", "Отпуска сотрудника (по ID)"),
                    new MenuItem("5", "Обновить отпуск"),
                    new MenuItem("6", "Удалить отпуск"),
                    new MenuItem("7", "Назад")
                };

                _menuManager.PrintMenu(menuItems);
                var choice = _menuManager.GetInput("Выберите действие");

                switch (choice)
                {
                    case "1":
                        AddVacation();
                        break;
                    case "2":
                        ViewAllVacations();
                        break;
                    case "3":
                        ViewVacationById();
                        break;
                    case "4":
                        ViewVacationsByEmployee();
                        break;
                    case "5":
                        UpdateVacation();
                        break;
                    case "6":
                        DeleteVacation();
                        break;
                    case "7":
                        return;
                    default:
                        _menuManager.PrintError("Неверный выбор");
                        break;
                }
            }
        }

        private void AddVacation()
        {
            _menuManager.PrintHeader("Добавление отпуска");

            var empIdStr = _menuManager.GetInput("ID сотрудника");
            if (!int.TryParse(empIdStr, out int empId))
            {
                _menuManager.PrintError("ID должен быть числом");
                _menuManager.WaitForKey();
                return;
            }

            var emp = _employeeService.GetEmployeeById(empId);
            if (emp == null)
            {
                _menuManager.PrintError($"Сотрудник с ID {empId} не найден");
                _menuManager.WaitForKey();
                return;
            }

            var _name = $"{emp.FirstName} {emp.LastName}".Trim();
            if (string.IsNullOrWhiteSpace(_name))
                _menuManager.PrintInfo($"Сотрудник с ID {empId}");
            else
                _menuManager.PrintInfo($"Сотрудник: {_name} (ID: {empId})");

            var startDateStr = _menuManager.GetInput("Дата начала отпуска (yyyy-MM-dd)");
            if (!DateTime.TryParse(startDateStr, out DateTime startDate))
            {
                _menuManager.PrintError("Неверный формат даты");
                _menuManager.WaitForKey();
                return;
            }

            var endDateStr = _menuManager.GetInput("Дата конца отпуска (yyyy-MM-dd)");
            if (!DateTime.TryParse(endDateStr, out DateTime endDate))
            {
                _menuManager.PrintError("Неверный формат даты");
                _menuManager.WaitForKey();
                return;
            }

            if (endDate <= startDate)
            {
                _menuManager.PrintError("Дата конца должна быть позже даты начала");
                _menuManager.WaitForKey();
                return;
            }

            var reason = _menuManager.GetInput("Причина отпуска");

            var vacation = new Vacation
            {
                EmployeeId = empId,
                StartDate = startDate,
                EndDate = endDate,
                Reason = reason,
                Status = "В ожидании"
            };

            _vacationService.AddVacation(vacation);
            var days = (endDate - startDate).Days;
            _menuManager.PrintSuccess($"Отпуск ID {vacation.Id} создан на {days} дней");
            _menuManager.WaitForKey();
        }

        private void ViewAllVacations()
        {
            _menuManager.PrintHeader("Все отпуска");

            var vacations = _vacationService.GetAllVacations();
            if (vacations.Count == 0)
            {
                _menuManager.PrintInfo("Нет записей об отпусках");
                _menuManager.WaitForKey();
                return;
            }

            _menuManager.PrintSeparator();
            foreach (var vac in vacations)
            {
                var emp = _employeeService.GetEmployeeById(vac.EmployeeId);
                string empName;
                if (emp == null)
                    empName = "Сотрудник удален";
                else
                {
                    var _n = $"{emp.FirstName} {emp.LastName}".Trim();
                    empName = string.IsNullOrWhiteSpace(_n) ? $"ID:{vac.EmployeeId:D3}" : _n;
                }
                var days = (vac.EndDate - vac.StartDate).Days;
                
                _consoleHelper.WriteLine($"  ID: {vac.Id:D3} | Сотр.ID: {vac.EmployeeId:D3} | {empName,-20} | " +
                    $"{vac.StartDate:dd.MM.yyyy} - {vac.EndDate:dd.MM.yyyy} ({days}дн.) | {vac.Reason,-15} | {vac.Status}");
            }
            _menuManager.PrintSeparator();
            _menuManager.WaitForKey();
        }

        private void ViewVacationById()
        {
            _menuManager.PrintHeader("Поиск отпуска по ID");

            var vacIdStr = _menuManager.GetInput("ID отпуска");
            if (!int.TryParse(vacIdStr, out int vacId))
            {
                _menuManager.PrintError("ID должен быть числом");
                _menuManager.WaitForKey();
                return;
            }

            var vac = _vacationService.GetVacationById(vacId);
            if (vac == null)
            {
                _menuManager.PrintError($"Отпуск с ID {vacId} не найден");
            }
            else
            {
                var emp = _employeeService.GetEmployeeById(vac.EmployeeId);
                string empDisplay;
                if (emp == null)
                    empDisplay = "Сотрудник удален";
                else
                {
                    var _n = $"{emp.FirstName} {emp.LastName}".Trim();
                    empDisplay = string.IsNullOrWhiteSpace(_n) ? $"Сотрудник с ID {vac.EmployeeId}" : $"{_n} (ID: {vac.EmployeeId})";
                }
                var days = (vac.EndDate - vac.StartDate).Days;

                _menuManager.PrintInfo($"ID: {vac.Id} | Сотрудник: {empDisplay} | " +
                    $"Период: {vac.StartDate:dd.MM.yyyy} - {vac.EndDate:dd.MM.yyyy} ({days}дней) | " +
                    $"Причина: {vac.Reason} | Статус: {vac.Status}");
            }
            
            _menuManager.WaitForKey();
        }

        private void ViewVacationsByEmployee()
        {
            _menuManager.PrintHeader("Отпуска сотрудника");

            var empIdStr = _menuManager.GetInput("ID сотрудника");
            if (!int.TryParse(empIdStr, out int empId))
            {
                _menuManager.PrintError("ID должен быть числом");
                _menuManager.WaitForKey();
                return;
            }

            var emp = _employeeService.GetEmployeeById(empId);
            if (emp == null)
            {
                _menuManager.PrintError($"Сотрудник с ID {empId} не найден");
                _menuManager.WaitForKey();
                return;
            }

            var vacations = _vacationService.GetVacationsByEmployee(empId);
            if (vacations.Count == 0)
            {
                var _n = $"{emp.FirstName} {emp.LastName}".Trim();
                if (string.IsNullOrWhiteSpace(_n))
                    _menuManager.PrintInfo($"Отпусков для сотрудника с ID {empId} не найдено");
                else
                    _menuManager.PrintInfo($"Отпусков для сотрудника {_n} не найдено");
            }
            else
            {
                var _n = $"{emp.FirstName} {emp.LastName}".Trim();
                if (string.IsNullOrWhiteSpace(_n))
                    _menuManager.PrintInfo($"Отпуска сотрудника (ID: {empId})");
                else
                    _menuManager.PrintInfo($"Отпуска сотрудника: {_n} (ID: {empId})");
                _menuManager.PrintSeparator();
                foreach (var vac in vacations)
                {
                    var days = (vac.EndDate - vac.StartDate).Days;
                    _consoleHelper.WriteLine($"  ID: {vac.Id:D3} | {vac.StartDate:dd.MM.yyyy} - " +
                        $"{vac.EndDate:dd.MM.yyyy} ({days}дн.) | {vac.Reason,-15} | {vac.Status}");
                }
                _menuManager.PrintSeparator();
            }
            
            _menuManager.WaitForKey();
        }

        private void UpdateVacation()
        {
            _menuManager.PrintHeader("Обновление отпуска");

            var vacIdStr = _menuManager.GetInput("ID отпуска");
            if (!int.TryParse(vacIdStr, out int vacId))
            {
                _menuManager.PrintError("ID должен быть числом");
                _menuManager.WaitForKey();
                return;
            }

            var vac = _vacationService.GetVacationById(vacId);
            if (vac == null)
            {
                _menuManager.PrintError($"Отпуск с ID {vacId} не найден");
                _menuManager.WaitForKey();
                return;
            }

            _menuManager.PrintInfo($"Обновление отпуска ID {vacId}");

            var newStartStr = _menuManager.GetInput($"Новая дата начала (текущая: {vac.StartDate:dd.MM.yyyy})");
            if (DateTime.TryParse(newStartStr, out DateTime newStart))
                vac.StartDate = newStart;

            var newEndStr = _menuManager.GetInput($"Новая дата конца (текущая: {vac.EndDate:dd.MM.yyyy})");
            if (DateTime.TryParse(newEndStr, out DateTime newEnd))
                vac.EndDate = newEnd;

            _consoleHelper.Write($"  ➤ Новая причина (текущая: {vac.Reason}): ");
            var newReason = _consoleHelper.ReadLine();
            if (!string.IsNullOrEmpty(newReason)) vac.Reason = newReason;

            _vacationService.UpdateVacation(vac);
            _menuManager.PrintSuccess($"Отпуск ID {vacId} обновлен");
            _menuManager.WaitForKey();
        }

        private void DeleteVacation()
        {
            _menuManager.PrintHeader("Удаление отпуска");

            var vacIdStr = _menuManager.GetInput("ID отпуска для удаления");
            if (!int.TryParse(vacIdStr, out int vacId))
            {
                _menuManager.PrintError("ID должен быть числом");
                _menuManager.WaitForKey();
                return;
            }

            var vac = _vacationService.GetVacationById(vacId);
            if (vac == null)
            {
                _menuManager.PrintError($"Отпуск с ID {vacId} не найден");
                _menuManager.WaitForKey();
                return;
            }

            _consoleHelper.WriteLine($"  ⚠ Вы уверены, что хотите удалить отпуск ID {vacId}? (y/n)");
            var confirm = _consoleHelper.ReadLine()?.ToLower();
            
            if (confirm == "y" || confirm == "yes")
            {
                _vacationService.DeleteVacation(vacId);
                _menuManager.PrintSuccess($"Отпуск ID {vacId} удален");
            }
            else
            {
                _menuManager.PrintInfo("Удаление отменено");
            }
            
            _menuManager.WaitForKey();
        }
    }
}

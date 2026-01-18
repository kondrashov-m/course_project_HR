using HRSystem.Models;
using HRSystem.Services;
using HRSystem.Utils;
using System;
using System.Collections.Generic;

namespace HRSystem.Commands
{
    /// <summary>
    /// Команда для управления зарплатой с ID-привязкой.
    /// </summary>
    public class SalaryCommand : ICommand
    {
        private readonly ISalaryService _salaryService;
        private readonly IEmployeeService _employeeService;
        private readonly IConsoleHelper _consoleHelper;
        private readonly MenuManager _menuManager;

        public SalaryCommand(ISalaryService salaryService, IEmployeeService employeeService, IConsoleHelper consoleHelper)
        {
            _salaryService = salaryService;
            _employeeService = employeeService;
            _consoleHelper = consoleHelper;
            _menuManager = new MenuManager(consoleHelper);
        }

        public string Name => "Зарплата";

        public void Execute()
        {
            while (true)
            {
                var menuItems = new List<MenuItem>
                {
                    new MenuItem("1", "Добавить запись о зарплате"),
                    new MenuItem("2", "Просмотреть все зарплаты"),
                    new MenuItem("3", "Найти запись по ID"),
                    new MenuItem("4", "Зарплата сотрудника (по ID)"),
                    new MenuItem("5", "Обновить запись"),
                    new MenuItem("6", "Удалить запись"),
                    new MenuItem("7", "Расчеты"),
                    new MenuItem("8", "Назад")
                };

                _menuManager.PrintMenu(menuItems);
                var choice = _menuManager.GetInput("Выберите действие");

                switch (choice)
                {
                    case "1":
                        AddSalary();
                        break;
                    case "2":
                        ViewAllSalaries();
                        break;
                    case "3":
                        ViewSalaryById();
                        break;
                    case "4":
                        ViewSalaryByEmployee();
                        break;
                    case "5":
                        UpdateSalary();
                        break;
                    case "6":
                        DeleteSalary();
                        break;
                    case "7":
                        ShowCalculations();
                        break;
                    case "8":
                        return;
                    default:
                        _menuManager.PrintError("Неверный выбор");
                        break;
                }
            }
        }

        private void AddSalary()
        {
            _menuManager.PrintHeader("Добавление записи о зарплате");

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

            var baseSalaryStr = _menuManager.GetInput("Базовая зарплата");
            if (!decimal.TryParse(baseSalaryStr, out decimal baseSalary) || baseSalary < 0)
            {
                _menuManager.PrintError("Зарплата должна быть положительным числом");
                _menuManager.WaitForKey();
                return;
            }

            var bonusStr = _menuManager.GetInput("Бонус (по умолчанию 0)");
            decimal.TryParse(bonusStr, out decimal bonus);
            if (bonus < 0) bonus = 0;

            var deductionsStr = _menuManager.GetInput("Вычеты (по умолчанию 0)");
            decimal.TryParse(deductionsStr, out decimal deductions);
            if (deductions < 0) deductions = 0;

            var salary = new Salary
            {
                EmployeeId = empId,
                BaseSalary = baseSalary,
                Bonus = bonus,
                Deductions = deductions,
                PaymentDate = DateTime.Now
            };

            _salaryService.AddSalary(salary);
            var total = baseSalary + bonus - deductions;
            _menuManager.PrintSuccess($"Запись ID {salary.Id} создана. Итого к выплате: {total:F2}");
            _menuManager.WaitForKey();
        }

        private void ViewAllSalaries()
        {
            _menuManager.PrintHeader("Все записи о зарплате");

            var salaries = _salaryService.GetAllSalaries();
            if (salaries.Count == 0)
            {
                _menuManager.PrintInfo("Нет записей о зарплате");
                _menuManager.WaitForKey();
                return;
            }

            _menuManager.PrintSeparator();
            foreach (var sal in salaries)
            {
                var emp = _employeeService.GetEmployeeById(sal.EmployeeId);
                string empName;
                if (emp == null)
                    empName = "Сотрудник удален";
                else
                {
                    var _n = $"{emp.FirstName} {emp.LastName}".Trim();
                    empName = string.IsNullOrWhiteSpace(_n) ? $"ID:{sal.EmployeeId:D3}" : _n;
                }
                var total = sal.BaseSalary + sal.Bonus - sal.Deductions;
                
                    _consoleHelper.WriteLine($"  ID: {sal.Id:D3} | {empName,-20} | База: {HRSystem.Utils.AppSettings.FormatMoney(sal.BaseSalary)} | " +
                    $"Бонус: {HRSystem.Utils.AppSettings.FormatMoney(sal.Bonus)} | Вычеты: {HRSystem.Utils.AppSettings.FormatMoney(sal.Deductions)} | Итого: {HRSystem.Utils.AppSettings.FormatMoney(total)}");
            }
            _menuManager.PrintSeparator();
            _menuManager.WaitForKey();
        }

        private void ViewSalaryById()
        {
            _menuManager.PrintHeader("Поиск записи о зарплате по ID");

            var salaryIdStr = _menuManager.GetInput("ID записи");
            if (!int.TryParse(salaryIdStr, out int salaryId))
            {
                _menuManager.PrintError("ID должен быть числом");
                _menuManager.WaitForKey();
                return;
            }

            var sal = _salaryService.GetSalaryById(salaryId);
            if (sal == null)
            {
                _menuManager.PrintError($"Запись с ID {salaryId} не найдена");
            }
            else
            {
                var emp = _employeeService.GetEmployeeById(sal.EmployeeId);
                string empDisplay;
                if (emp == null)
                    empDisplay = "Сотрудник удален";
                else
                {
                    var _n = $"{emp.FirstName} {emp.LastName}".Trim();
                    empDisplay = string.IsNullOrWhiteSpace(_n) ? $"Сотрудник с ID {sal.EmployeeId}" : $"{_n} (ID: {sal.EmployeeId})";
                }
                var total = sal.BaseSalary + sal.Bonus - sal.Deductions;
                
                _menuManager.PrintInfo($"ID: {sal.Id} | Сотрудник: {empDisplay} | " +
                    $"База: {HRSystem.Utils.AppSettings.FormatMoney(sal.BaseSalary)} | Бонус: {HRSystem.Utils.AppSettings.FormatMoney(sal.Bonus)} | Вычеты: {HRSystem.Utils.AppSettings.FormatMoney(sal.Deductions)} | " +
                    $"Итого: {HRSystem.Utils.AppSettings.FormatMoney(total)} | Дата: {sal.PaymentDate:dd.MM.yyyy}");
            }
            
            _menuManager.WaitForKey();
        }

        private void ViewSalaryByEmployee()
        {
            _menuManager.PrintHeader("Зарплата сотрудника");

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

            var salaries = _salaryService.GetSalariesByEmployee(empId);
            if (salaries.Count == 0)
            {
                var _n = $"{emp.FirstName} {emp.LastName}".Trim();
                if (string.IsNullOrWhiteSpace(_n))
                    _menuManager.PrintInfo($"Записей о зарплате для сотрудника с ID {empId} не найдено");
                else
                    _menuManager.PrintInfo($"Записей о зарплате для {_n} не найдено");
            }
            else
            {
                var _n = $"{emp.FirstName} {emp.LastName}".Trim();
                if (string.IsNullOrWhiteSpace(_n))
                    _menuManager.PrintInfo($"Зарплата (ID: {empId})");
                else
                    _menuManager.PrintInfo($"Зарплата: {_n} (ID: {empId})");
                _menuManager.PrintSeparator();
                foreach (var sal in salaries)
                {
                    var total = sal.BaseSalary + sal.Bonus - sal.Deductions;
                    _consoleHelper.WriteLine($"  ID: {sal.Id:D3} | База: {sal.BaseSalary:F2} | " +
                        $"Бонус: {sal.Bonus:F2} | Вычеты: {sal.Deductions:F2} | Итого: {total:F2}");
                }
                _menuManager.PrintSeparator();
            }
            
            _menuManager.WaitForKey();
        }

        private void UpdateSalary()
        {
            _menuManager.PrintHeader("Обновление записи о зарплате");

            var salaryIdStr = _menuManager.GetInput("ID записи");
            if (!int.TryParse(salaryIdStr, out int salaryId))
            {
                _menuManager.PrintError("ID должен быть числом");
                _menuManager.WaitForKey();
                return;
            }

            var sal = _salaryService.GetSalaryById(salaryId);
            if (sal == null)
            {
                _menuManager.PrintError($"Запись с ID {salaryId} не найдена");
                _menuManager.WaitForKey();
                return;
            }

            _menuManager.PrintInfo($"Обновление записи ID {salaryId}");

            _consoleHelper.Write($"  ➤ Новая базовая зарплата (текущая: {sal.BaseSalary:F2}): ");
            if (decimal.TryParse(_consoleHelper.ReadLine(), out decimal newBase))
                sal.BaseSalary = newBase;

            _consoleHelper.Write($"  ➤ Новый бонус (текущий: {sal.Bonus:F2}): ");
            if (decimal.TryParse(_consoleHelper.ReadLine(), out decimal newBonus))
                sal.Bonus = newBonus;

            _consoleHelper.Write($"  ➤ Новые вычеты (текущие: {sal.Deductions:F2}): ");
            if (decimal.TryParse(_consoleHelper.ReadLine(), out decimal newDeductions))
                sal.Deductions = newDeductions;

            _salaryService.UpdateSalary(sal);
            _menuManager.PrintSuccess($"Запись ID {salaryId} обновлена");
            _menuManager.WaitForKey();
        }

        private void DeleteSalary()
        {
            _menuManager.PrintHeader("Удаление записи о зарплате");

            var salaryIdStr = _menuManager.GetInput("ID записи для удаления");
            if (!int.TryParse(salaryIdStr, out int salaryId))
            {
                _menuManager.PrintError("ID должен быть числом");
                _menuManager.WaitForKey();
                return;
            }

            var sal = _salaryService.GetSalaryById(salaryId);
            if (sal == null)
            {
                _menuManager.PrintError($"Запись с ID {salaryId} не найдена");
                _menuManager.WaitForKey();
                return;
            }

            _consoleHelper.WriteLine($"  ⚠ Вы уверены, что хотите удалить запись ID {salaryId}? (y/n)");
            var confirm = _consoleHelper.ReadLine()?.ToLower();
            
            if (confirm == "y" || confirm == "yes")
            {
                _salaryService.DeleteSalary(salaryId);
                _menuManager.PrintSuccess($"Запись ID {salaryId} удалена");
            }
            else
            {
                _menuManager.PrintInfo("Удаление отменено");
            }
            
            _menuManager.WaitForKey();
        }

        private void ShowCalculations()
        {
            _menuManager.PrintHeader("Расчеты");

            var salaries = _salaryService.GetAllSalaries();
            if (salaries.Count == 0)
            {
                _menuManager.PrintInfo("Нет данных для расчета");
                _menuManager.WaitForKey();
                return;
            }

            decimal totalBase = 0, totalBonus = 0, totalDeductions = 0;
            foreach (var sal in salaries)
            {
                totalBase += sal.BaseSalary;
                totalBonus += sal.Bonus;
                totalDeductions += sal.Deductions;
            }

            decimal totalPayout = totalBase + totalBonus - totalDeductions;
            decimal avgSalary = totalBase / salaries.Count;

            _menuManager.PrintSeparator();
            _consoleHelper.WriteLine($"  Количество записей: {salaries.Count}");
            _consoleHelper.WriteLine($"  Сумма базовых зарплат: {totalBase:F2}");
            _consoleHelper.WriteLine($"  Сумма бонусов: {totalBonus:F2}");
            _consoleHelper.WriteLine($"  Сумма вычетов: {totalDeductions:F2}");
            _consoleHelper.WriteLine($"  ИТОГО К ВЫПЛАТЕ: {totalPayout:F2}");
            _consoleHelper.WriteLine($"  Средняя зарплата: {avgSalary:F2}");
            _menuManager.PrintSeparator();
            
            _menuManager.WaitForKey();
        }
    }
}

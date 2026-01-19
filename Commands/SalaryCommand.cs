using HRSystem.Services;
using HRSystem.UI;

namespace HRSystem.Commands
{
    public class SalaryCommand : ICommand
    {
        private readonly ISalaryService _salaryService;
        private readonly IEmployeeService _employeeService;
        private readonly IConsoleManager _console;
        
        public SalaryCommand(ISalaryService salaryService, IEmployeeService employeeService, IConsoleManager console)
        {
            _salaryService = salaryService;
            _employeeService = employeeService;
            _console = console;
        }
        
        public void Execute()
        {
            while (true)
            {
                _console.ClearScreen();
                _console.PrintHeader("Управление зарплатой");
                
                _console.PrintMenu(new List<MenuItem>
                {
                    new("1", "Рассчитать зарплату"),
                    new("2", "История выплат"),
                    new("3", "Назад")
                });
                
                var choice = _console.GetInput("Выберите");
                
                switch (choice)
                {
                    case "1": CalculateSalary(); break;
                    case "2": SalaryHistory(); break;
                    case "3": return;
                    default: _console.PrintError("Неверный выбор"); _console.WaitForKey(); break;
                }
            }
        }
        
        private void CalculateSalary()
        {
            _console.ClearScreen();
            _console.PrintHeader("Расчет зарплаты");
            
            var employeeIdInput = _console.GetInput("ID сотрудника");
            var monthInput = _console.GetInput("Месяц (1-12)");
            var yearInput = _console.GetInput("Год");
            
            if (int.TryParse(employeeIdInput, out var employeeId) &&
                int.TryParse(monthInput, out var month) &&
                int.TryParse(yearInput, out var year))
            {
                var salary = _salaryService.CalculateSalary(employeeId, month, year);
                var employee = _employeeService.GetEmployee(employeeId);
                
                if (employee != null)
                {
                    _console.PrintInfo($"Сотрудник: {employee.FirstName} {employee.LastName}");
                    _console.PrintInfo($"Зарплата: {salary:C}");
                    _console.PrintInfo($"Месяц: {month}/{year}");
                }
                else
                {
                    _console.PrintError("Сотрудник не найден");
                }
            }
            else
            {
                _console.PrintError("Ошибка ввода");
            }
            
            _console.WaitForKey();
        }
        
        private void SalaryHistory()
        {
            _console.ClearScreen();
            _console.PrintHeader("История выплат");
            
            var employeeIdInput = _console.GetInput("ID сотрудника");
            
            if (int.TryParse(employeeIdInput, out var employeeId))
            {
                var history = _salaryService.GetSalaryHistory(employeeId);
                var employee = _employeeService.GetEmployee(employeeId);
                
                if (employee != null)
                {
                    _console.PrintInfo($"Сотрудник: {employee.FirstName} {employee.LastName}");
                }
                
                foreach (var record in history)
                {
                    _console.PrintInfo($"{record.Month:00}/{record.Year}: {record.Amount:C}");
                }
            }
            else
            {
                _console.PrintError("Ошибка ввода");
            }
            
            _console.WaitForKey();
        }
    }
}
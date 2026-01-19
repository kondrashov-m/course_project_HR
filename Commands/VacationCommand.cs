using HRSystem.Services;
using HRSystem.UI;

namespace HRSystem.Commands
{
    public class VacationCommand : ICommand
    {
        private readonly IVacationService _vacationService;
        private readonly IEmployeeService _employeeService;
        private readonly IConsoleManager _console;
        
        public VacationCommand(IVacationService vacationService, IEmployeeService employeeService, IConsoleManager console)
        {
            _vacationService = vacationService;
            _employeeService = employeeService;
            _console = console;
        }
        
        public void Execute()
        {
            while (true)
            {
                _console.ClearScreen();
                _console.PrintHeader("Управление отпусками");
                
                _console.PrintMenu(new List<MenuItem>
                {
                    new("1", "Добавить отпуск"),
                    new("2", "Список отпусков"),
                    new("3", "Утвердить отпуск"),
                    new("4", "Отклонить отпуск"),
                    new("5", "Назад")
                });
                
                var choice = _console.GetInput("Выберите");
                
                switch (choice)
                {
                    case "1": AddVacation(); break;
                    case "2": ListVacations(); break;
                    case "3": ApproveVacation(); break;
                    case "4": RejectVacation(); break;
                    case "5": return;
                    default: _console.PrintError("Неверный выбор"); _console.WaitForKey(); break;
                }
            }
        }
        
        private void AddVacation()
        {
            _console.ClearScreen();
            _console.PrintHeader("Добавление отпуска");
            
            var employeeIdInput = _console.GetInput("ID сотрудника");
            var startDateInput = _console.GetInput("Дата начала (гггг-мм-дд)");
            var endDateInput = _console.GetInput("Дата окончания (гггг-мм-дд)");
            
            if (int.TryParse(employeeIdInput, out var employeeId) &&
                DateTime.TryParse(startDateInput, out var startDate) &&
                DateTime.TryParse(endDateInput, out var endDate))
            {
                var vacation = _vacationService.CreateVacation(employeeId, startDate, endDate);
                _console.PrintSuccess($"Отпуск добавлен. ID: {vacation.Id}");
            }
            else
            {
                _console.PrintError("Ошибка ввода");
            }
            
            _console.WaitForKey();
        }
        
        private void ListVacations()
        {
            _console.ClearScreen();
            _console.PrintHeader("Список отпусков");
            
            var employeeIdInput = _console.GetInput("ID сотрудника");
            
            if (int.TryParse(employeeIdInput, out var employeeId))
            {
                var vacations = _vacationService.GetEmployeeVacations(employeeId);
                var employee = _employeeService.GetEmployee(employeeId);
                
                if (employee != null)
                {
                    _console.PrintInfo($"Сотрудник: {employee.FirstName} {employee.LastName}");
                }
                
                foreach (var vacation in vacations)
                {
                    _console.PrintInfo($"[{vacation.Id}] {vacation.StartDate:dd.MM.yyyy} - {vacation.EndDate:dd.MM.yyyy}");
                    _console.PrintInfo($"   Статус: {vacation.Status}");
                    _console.PrintInfo("");
                }
            }
            else
            {
                _console.PrintError("Ошибка ввода");
            }
            
            _console.WaitForKey();
        }
        
        private void ApproveVacation()
        {
            _console.ClearScreen();
            _console.PrintHeader("Утверждение отпуска");
            
            var vacationIdInput = _console.GetInput("ID отпуска");
            
            if (int.TryParse(vacationIdInput, out var vacationId))
            {
                var success = _vacationService.ApproveVacation(vacationId);
                if (success) _console.PrintSuccess("Отпуск утвержден");
                else _console.PrintError("Отпуск не найден");
            }
            else
            {
                _console.PrintError("Ошибка ввода");
            }
            
            _console.WaitForKey();
        }
        
        private void RejectVacation()
        {
            _console.ClearScreen();
            _console.PrintHeader("Отклонение отпуска");
            
            var vacationIdInput = _console.GetInput("ID отпуска");
            
            if (int.TryParse(vacationIdInput, out var vacationId))
            {
                var success = _vacationService.RejectVacation(vacationId);
                if (success) _console.PrintSuccess("Отпуск отклонен");
                else _console.PrintError("Отпуск не найден");
            }
            else
            {
                _console.PrintError("Ошибка ввода");
            }
            
            _console.WaitForKey();
        }
    }
}
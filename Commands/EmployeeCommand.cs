using HRSystem.Services;
using HRSystem.UI;

namespace HRSystem.Commands
{
    public class EmployeeCommand : ICommand
    {
        private readonly IEmployeeService _employeeService;
        private readonly IConsoleManager _console;
        
        public EmployeeCommand(IEmployeeService employeeService, IConsoleManager console)
        {
            _employeeService = employeeService;
            _console = console;
        }
        
        public void Execute()
        {
            while (true)
            {
                _console.ClearScreen();
                _console.PrintHeader("Управление сотрудниками");
                
                _console.PrintMenu(new List<MenuItem>
                {
                    new("1", "Добавить сотрудника"),
                    new("2", "Список сотрудников"),
                    new("3", "Изменить зарплату"),
                    new("4", "Удалить сотрудника"),
                    new("5", "Назад")
                });
                
                var choice = _console.GetInput("Выберите");
                
                switch (choice)
                {
                    case "1": AddEmployee(); break;
                    case "2": ListEmployees(); break;
                    case "3": UpdateSalary(); break;
                    case "4": DeleteEmployee(); break;
                    case "5": return;
                    default: _console.PrintError("Неверный выбор"); _console.WaitForKey(); break;
                }
            }
        }
        
        private void AddEmployee()
        {
            _console.ClearScreen();
            _console.PrintHeader("Добавление сотрудника");
            
            var firstName = _console.GetInput("Имя");
            var lastName = _console.GetInput("Фамилия");
            var position = _console.GetInput("Должность");
            var salaryInput = _console.GetInput("Зарплата");
            var departmentInput = _console.GetInput("ID отдела");
            
            if (decimal.TryParse(salaryInput, out var salary) && 
                int.TryParse(departmentInput, out var departmentId))
            {
                var employee = _employeeService.CreateEmployee(firstName, lastName, position, salary, departmentId);
                _console.PrintSuccess($"Сотрудник добавлен. ID: {employee.Id}");
            }
            else
            {
                _console.PrintError("Ошибка ввода");
            }
            
            _console.WaitForKey();
        }
        
        private void ListEmployees()
        {
            _console.ClearScreen();
            _console.PrintHeader("Список сотрудников");
            
            var employees = _employeeService.GetAllEmployees();
            foreach (var emp in employees)
            {
                _console.PrintInfo($"[{emp.Id}] {emp.FirstName} {emp.LastName}");
                _console.PrintInfo($"   Должность: {emp.Position}");
                _console.PrintInfo($"   Зарплата: {emp.Salary:C}");
                _console.PrintInfo($"   Отдел: {emp.DepartmentId}");
                _console.PrintInfo("");
            }
            
            _console.PrintInfo($"Всего: {employees.Count} сотрудников");
            _console.WaitForKey();
        }
        
        private void UpdateSalary()
        {
            _console.ClearScreen();
            _console.PrintHeader("Изменение зарплаты");
            
            var idInput = _console.GetInput("ID сотрудника");
            var salaryInput = _console.GetInput("Новая зарплата");
            
            if (int.TryParse(idInput, out var id) && 
                decimal.TryParse(salaryInput, out var salary))
            {
                _employeeService.UpdateEmployeeSalary(id, salary);
                _console.PrintSuccess("Зарплата обновлена");
            }
            else
            {
                _console.PrintError("Ошибка ввода");
            }
            
            _console.WaitForKey();
        }
        
        private void DeleteEmployee()
        {
            _console.ClearScreen();
            _console.PrintHeader("Удаление сотрудника");
            
            var idInput = _console.GetInput("ID сотрудника");
            
            if (int.TryParse(idInput, out var id))
            {
                var success = _employeeService.DeleteEmployee(id);
                if (success) _console.PrintSuccess("Сотрудник удален");
                else _console.PrintError("Сотрудник не найден");
            }
            else
            {
                _console.PrintError("Ошибка ввода");
            }
            
            _console.WaitForKey();
        }
    }
}
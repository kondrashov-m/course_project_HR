using HRSystem.Commands;
using HRSystem.Repositories;
using HRSystem.Services;
using HRSystem.UI;

namespace HRSystem.Core
{
    public class HRApplication
    {
        private readonly IConsoleManager _console;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IEmployeeService _employeeService;
        private readonly IVacationService _vacationService;
        private readonly ISalaryService _salaryService;
        private readonly EmployeeCommand _employeeCommand;
        private readonly VacationCommand _vacationCommand;
        private readonly SalaryCommand _salaryCommand;
        
        public HRApplication()
        {
            _console = new ConsoleManager();
            _employeeRepository = new JsonEmployeeRepository();
            _employeeService = new EmployeeService(_employeeRepository);
            _vacationService = new VacationService(_employeeRepository);
            _salaryService = new SalaryService(_employeeRepository);
            _employeeCommand = new EmployeeCommand(_employeeService, _console);
            _vacationCommand = new VacationCommand(_vacationService, _employeeService, _console);
            _salaryCommand = new SalaryCommand(_salaryService, _employeeService, _console);
        }
        
        public void Run()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            
            while (true)
            {
                _console.ClearScreen();
                PrintMainHeader();
                
                _console.PrintMenu(new List<MenuItem>
                {
                    new("1", "Управление сотрудниками"),
                    new("2", "Управление отпусками"),
                    new("3", "Управление зарплатой"),
                    new("4", "Отчеты"),
                    new("5", "Выход")
                });
                
                var choice = _console.GetInput("Выберите пункт меню");
                
                switch (choice)
                {
                    case "1": _employeeCommand.Execute(); break;
                    case "2": _vacationCommand.Execute(); break;
                    case "3": _salaryCommand.Execute(); break;
                    case "4": ShowReports(); break;
                    case "5": Exit(); return;
                    default: _console.PrintError("Неверный выбор"); _console.WaitForKey(); break;
                }
            }
        }
        
        private void PrintMainHeader()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("╔════════════════════════════════════════════════════╗");
            Console.WriteLine("║                                                    ║");
            Console.WriteLine("║               HR SYSTEM v1.0                      ║");
            Console.WriteLine("║               by kondrashov-m                     ║");
            Console.WriteLine("║               Кондрашов Михаил Иванович           ║");
            Console.WriteLine("║                                                    ║");
            Console.WriteLine("╚════════════════════════════════════════════════════╝");
            Console.ResetColor();
            Console.WriteLine();
        }
        
        private void ShowReports()
        {
            _console.ClearScreen();
            _console.PrintHeader("Отчеты");
            
            var employees = _employeeService.GetAllEmployees();
            var totalSalary = employees.Sum(e => e.Salary);
            
            // ЗАМЕНИТЕ ЭТУ СТРОКУ:
            // _console.PrintData("Общая статистика", "");
            // НА ЭТУ:
            _console.PrintInfo("Общая статистика:");
            
            _console.PrintInfo($"Всего сотрудников: {employees.Count}");
            _console.PrintInfo($"Общий фонд зарплат: {totalSalary:C}");
            _console.PrintInfo($"Средняя зарплата: {(employees.Count > 0 ? totalSalary / employees.Count : 0):C}");
            
            _console.WaitForKey();
        }
        
        private void Exit()
        {
            _console.ClearScreen();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("╔════════════════════════════════════════════════════╗");
            Console.WriteLine("║                                                    ║");
            Console.WriteLine("║          Спасибо за использование!                ║");
            Console.WriteLine("║          HR System by kondrashov-m                ║");
            Console.WriteLine("║          Кондрашов Михаил Иванович                ║");
            Console.WriteLine("║                                                    ║");
            Console.WriteLine("╚════════════════════════════════════════════════════╝");
            Console.ResetColor();
            Environment.Exit(0);
        }
    }
}
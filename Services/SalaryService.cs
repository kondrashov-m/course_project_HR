using HRSystem.Models;
using HRSystem.Repositories;

namespace HRSystem.Services
{
    public class SalaryService : ISalaryService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly JsonSalaryRepository _salaryRepository;
        
        public SalaryService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
            _salaryRepository = new JsonSalaryRepository();
        }
        
        public decimal CalculateSalary(int employeeId, int month, int year)
        {
            var employee = _employeeRepository.GetById(employeeId);
            if (employee == null) return 0;
            
            var existing = _salaryRepository.GetByEmployeeAndDate(employeeId, month, year);
            if (existing != null)
            {
                return existing.Amount;
            }
            
            decimal baseSalary = employee.Salary;
            decimal tax = baseSalary * 0.13m;
            decimal netSalary = baseSalary - tax;
            
            var salaryRecord = new Salary
            {
                EmployeeId = employeeId,
                Month = month,
                Year = year,
                Amount = netSalary,
                CalculationDate = DateTime.Now
            };
            
            _salaryRepository.Add(salaryRecord);
            return netSalary;
        }
        
        public Salary GetSalaryRecord(int employeeId, int month, int year)
        {
            return _salaryRepository.GetByEmployeeAndDate(employeeId, month, year);
        }
        
        public List<Salary> GetSalaryHistory(int employeeId)
        {
            return _salaryRepository.GetByEmployeeId(employeeId);
        }
        
        public List<Salary> GetAllSalaries()
        {
            return _salaryRepository.GetAll();
        }
    }
}
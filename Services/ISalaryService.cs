using HRSystem.Models;

namespace HRSystem.Services
{
    public interface ISalaryService
    {
        decimal CalculateSalary(int employeeId, int month, int year);
        Salary GetSalaryRecord(int employeeId, int month, int year);
        List<Salary> GetSalaryHistory(int employeeId);
    }
}
using System.Collections.Generic;
using HRSystem.Models;

namespace HRSystem.Services
{
    /// <summary>
    /// Интерфейс для управления зарплатой.
    /// </summary>
    public interface ISalaryService
    {
        List<Salary> GetAllSalaries();
        Salary GetSalaryById(int id);
        void AddSalary(Salary salary);
        void UpdateSalary(Salary salary);
        void DeleteSalary(int id);
        List<Salary> GetSalariesByEmployee(int employeeId);
        decimal CalculateTotalSalary(int employeeId);
    }
}

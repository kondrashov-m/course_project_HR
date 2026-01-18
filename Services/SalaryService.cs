using System;
using System.Collections.Generic;
using System.Linq;
using HRSystem.Models;

namespace HRSystem.Services
{
    /// <summary>
    /// Сервис для управления зарплатой.
    /// </summary>
    public class SalaryService : ISalaryService
    {
        private static List<Salary> _salaries = new List<Salary>();
        private static int _nextId = 1;

        public List<Salary> GetAllSalaries() => _salaries.ToList();

        public Salary GetSalaryById(int id) => _salaries.FirstOrDefault(s => s.Id == id);

        public void AddSalary(Salary salary)
        {
            salary.Id = _nextId++;
            _salaries.Add(salary);
        }

        public void UpdateSalary(Salary salary)
        {
            var existing = _salaries.FirstOrDefault(s => s.Id == salary.Id);
            if (existing != null)
            {
                existing.EmployeeId = salary.EmployeeId;
                existing.BaseSalary = salary.BaseSalary;
                existing.Bonus = salary.Bonus;
                existing.Deductions = salary.Deductions;
                existing.PaymentDate = salary.PaymentDate;
            }
        }

        public void DeleteSalary(int id)
        {
            var salary = _salaries.FirstOrDefault(s => s.Id == id);
            if (salary != null)
                _salaries.Remove(salary);
        }

        public List<Salary> GetSalariesByEmployee(int employeeId)
        {
            return _salaries.Where(s => s.EmployeeId == employeeId).ToList();
        }

        public decimal CalculateTotalSalary(int employeeId)
        {
            var salaries = GetSalariesByEmployee(employeeId);
            return salaries.Sum(s => s.BaseSalary + s.Bonus - s.Deductions);
        }
    }
}

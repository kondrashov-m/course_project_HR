using System;
using System.Collections.Generic;
using System.Linq;
using HRSystem.Models;
using HRSystem.Repositories;

namespace HRSystem.Services
{
    /// <summary>
    /// Сервис для управления зарплатой.
    /// </summary>
    public class SalaryService : ISalaryService
    {
        private readonly IEmployeeRepository _repository;

        public SalaryService(IEmployeeRepository repository)
        {
            _repository = repository;
        }

        public List<Salary> GetAllSalaries() => _repository.GetAllSalaries();

        public Salary GetSalaryById(int id) => _repository.GetSalaryById(id);

        public void AddSalary(Salary salary)
        {
            _repository.AddSalary(salary);
        }

        public void UpdateSalary(Salary salary)
        {
            _repository.UpdateSalary(salary);
        }

        public void DeleteSalary(int id)
        {
            _repository.DeleteSalary(id);
        }

        public List<Salary> GetSalariesByEmployee(int employeeId)
        {
            return _repository.GetSalariesByEmployeeId(employeeId);
        }

        public decimal CalculateTotalSalary(int employeeId)
        {
            var salaries = _repository.GetSalariesByEmployeeId(employeeId);
            return salaries.Sum(s => s.BaseSalary + s.Bonus - s.Deductions);
        }
    }
}

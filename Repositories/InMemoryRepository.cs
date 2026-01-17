using System;
using System.Collections.Generic;
using System.Linq;

namespace HRSystem.Repositories
{
    /// <summary>
    /// –епозиторий дл€ хранени€ сотрудников в пам€ти.
    /// </summary>
    public class InMemoryRepository : IEmployeeRepository
    {
        private static readonly List<Employee> _employees = new();
        private static int _nextId = 1;

        public List<Employee> GetAll() => _employees.ToList();

        public Employee GetById(int id) => _employees.FirstOrDefault(e => e.Id == id);

        public void Add(Employee employee)
        {
            employee.Id = _nextId++;
            _employees.Add(employee);
        }

        public void Update(Employee employee)
        {
            var existing = _employees.FirstOrDefault(e => e.Id == employee.Id);
            if (existing != null)
            {
                existing.FirstName = employee.FirstName;
                existing.LastName = employee.LastName;
                existing.PositionName = employee.PositionName;
                existing.DepartmentName = employee.DepartmentName;
                existing.BaseSalary = employee.BaseSalary;
                existing.HireDate = employee.HireDate;
            }
        }

        public void Delete(int id)
        {
            var emp = _employees.FirstOrDefault(e => e.Id == id);
            if (emp != null)
                _employees.Remove(emp);
        }

        public int GetNextId() => _nextId;
    }
}

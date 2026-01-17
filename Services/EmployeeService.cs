using System;
using System.Collections.Generic;
using System.Linq;

namespace HRSystem.Services
{
    /// <summary>
    /// Сервис для управления сотрудниками.
    /// </summary>
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _repository;

        public EmployeeService(IEmployeeRepository repository)
        {
            _repository = repository;
        }

        public List<Employee> GetAllEmployees() => _repository.GetAll();

        public Employee GetEmployeeById(int id) => _repository.GetById(id);

        public void AddEmployee(Employee employee)
        {
            employee.Id = _repository.GetNextId();
            _repository.Add(employee);
        }

        public void UpdateEmployee(Employee employee)
        {
            var existing = _repository.GetById(employee.Id);
            if (existing != null)
            {
                existing.FirstName = employee.FirstName;
                existing.LastName = employee.LastName;
                existing.PositionName = employee.PositionName;
                existing.DepartmentName = employee.DepartmentName;
                existing.BaseSalary = employee.BaseSalary;
                existing.HireDate = employee.HireDate;
                _repository.Update(employee);
            }
        }

        public void DeleteEmployee(int id)
        {
            _repository.Delete(id);
        }

        public List<Employee> GetEmployeesByDepartment(string departmentName)
        {
            return _repository.GetAll().Where(e => e.DepartmentName == departmentName).ToList();
        }

        public List<Employee> GetEmployeesByPosition(string positionName)
        {
            return _repository.GetAll().Where(e => e.PositionName == positionName).ToList();
        }
    }
}

using HRSystem.Models;
using HRSystem.Repositories;

namespace HRSystem.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _repository;
        
        public EmployeeService(IEmployeeRepository repository)
        {
            _repository = repository;
        }
        
        public Employee CreateEmployee(string firstName, string lastName, string position, decimal salary, int departmentId)
        {
            var employee = new Employee
            {
                FirstName = firstName,
                LastName = lastName,
                Position = position,
                Salary = salary,
                DepartmentId = departmentId,
                HireDate = DateTime.Now
            };
            return _repository.Add(employee);
        }
        
        public Employee GetEmployee(int id)
        {
            return _repository.GetById(id);
        }
        
        public List<Employee> GetAllEmployees()
        {
            return _repository.GetAll();
        }
        
        public void UpdateEmployeeSalary(int id, decimal salary)
        {
            var employee = _repository.GetById(id);
            if (employee != null)
            {
                employee.Salary = salary;
                _repository.Update(employee);
            }
        }
        
        public bool DeleteEmployee(int id)
        {
            return _repository.Delete(id);
        }
    }
}
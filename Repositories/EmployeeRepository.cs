using HRSystem.Models;

namespace HRSystem.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly List<Employee> _employees = new();
        private int _nextId = 1;
        
        public Employee Add(Employee employee)
        {
            employee.Id = _nextId++;
            _employees.Add(employee);
            return employee;
        }
        
        public Employee GetById(int id)
        {
            return _employees.FirstOrDefault(e => e.Id == id);
        }
        
        public List<Employee> GetAll()
        {
            return _employees;
        }
        
        public void Update(Employee employee)
        {
            var existing = GetById(employee.Id);
            if (existing != null)
            {
                existing.FirstName = employee.FirstName;
                existing.LastName = employee.LastName;
                existing.Position = employee.Position;
                existing.Salary = employee.Salary;
                existing.DepartmentId = employee.DepartmentId;
            }
        }
        
        public bool Delete(int id)
        {
            var employee = GetById(id);
            return employee != null && _employees.Remove(employee);
        }
    }
}
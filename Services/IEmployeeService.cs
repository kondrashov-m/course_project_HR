using HRSystem.Models;

namespace HRSystem.Services
{
    public interface IEmployeeService
    {
        Employee CreateEmployee(string firstName, string lastName, string position, decimal salary, int departmentId);
        Employee GetEmployee(int id);
        List<Employee> GetAllEmployees();
        void UpdateEmployeeSalary(int id, decimal salary);
        bool DeleteEmployee(int id);
    }
}
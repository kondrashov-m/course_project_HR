using System.Collections.Generic;

namespace HRSystem.Services
{
    /// <summary>
    /// Интерфейс для управления сотрудниками.
    /// </summary>
    public interface IEmployeeService
    {
        List<Employee> GetAllEmployees();
        Employee GetEmployeeById(int id);
        void AddEmployee(Employee employee);
        void UpdateEmployee(Employee employee);
        void DeleteEmployee(int id);
        List<Employee> GetEmployeesByDepartment(string departmentName);
        List<Employee> GetEmployeesByPosition(string positionName);
    }
}

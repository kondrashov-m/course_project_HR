using System.Collections.Generic;
using HRSystem.Models;

namespace HRSystem.Services
{
    /// <summary>
    /// ��������� ��� ���������� ������������.
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

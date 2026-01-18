using System.Collections.Generic;
using HRSystem.Models;

namespace HRSystem.Services
{
    /// <summary>
    /// Интерфейс для управления отделами.
    /// </summary>
    public interface IDepartmentService
    {
        List<Department> GetAllDepartments();
        Department GetDepartmentById(int id);
        void AddDepartment(Department department);
        void UpdateDepartment(Department department);
        void DeleteDepartment(int id);
        List<Employee> GetEmployeesByDepartment(string departmentName);
    }
}

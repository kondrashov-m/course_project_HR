using System;
using System.Collections.Generic;
using System.Linq;
using HRSystem.Models;

namespace HRSystem.Services
{
    /// <summary>
    /// Сервис для управления отделами.
    /// </summary>
    public class DepartmentServiceImpl : IDepartmentService
    {
        private static List<Department> _departments = new List<Department>();
        private static int _nextId = 1;

        public DepartmentServiceImpl()
        {
            // Инициализируем стандартные отделы
            if (_departments.Count == 0)
            {
                AddDepartment(new Department { Name = "IT отдел", Description = "Информационные технологии" });
                AddDepartment(new Department { Name = "HR отдел", Description = "Управление персоналом" });
                AddDepartment(new Department { Name = "Финансы", Description = "Финансовый отдел" });
                AddDepartment(new Department { Name = "Маркетинг", Description = "Маркетинговый отдел" });
            }
        }

        public List<Department> GetAllDepartments() => _departments.ToList();

        public Department GetDepartmentById(int id) => _departments.FirstOrDefault(d => d.Id == id);

        public void AddDepartment(Department department)
        {
            department.Id = _nextId++;
            _departments.Add(department);
        }

        public void UpdateDepartment(Department department)
        {
            var existing = _departments.FirstOrDefault(d => d.Id == department.Id);
            if (existing != null)
            {
                existing.Name = department.Name;
                existing.Description = department.Description;
            }
        }

        public void DeleteDepartment(int id)
        {
            var department = _departments.FirstOrDefault(d => d.Id == id);
            if (department != null)
                _departments.Remove(department);
        }

        public List<Employee> GetEmployeesByDepartment(string departmentName)
        {
            // Это должно быть интегрировано с EmployeeService
            return new List<Employee>();
        }
    }
}

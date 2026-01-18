using System;
using System.Collections.Generic;
using System.Linq;
using HRSystem.Models;

namespace HRSystem.Repositories
{
    /// <summary>
    /// Расширенное хранилище для работы с сотрудниками.
    /// </summary>
    public class EmployeeRepositoryEx : BaseRepository<Employee>
    {
        protected override int GetId(Employee item) => item.Id;
        protected override void SetId(Employee item, int id) => item.Id = id;

        public List<Employee> GetByDepartment(string departmentName)
        {
            return _items.Where(e => e.DepartmentName == departmentName).ToList();
        }

        public List<Employee> GetByPosition(string positionName)
        {
            return _items.Where(e => e.PositionName == positionName).ToList();
        }

        public List<Employee> GetByName(string firstName, string lastName)
        {
            return _items.Where(e => 
                e.FirstName == firstName && e.LastName == lastName).ToList();
        }

        public Employee GetByFullName(string fullName)
        {
            var parts = fullName.Split(' ');
            if (parts.Length >= 2)
            {
                return _items.FirstOrDefault(e => 
                    e.FirstName == parts[0] && e.LastName == parts[1]);
            }
            return null;
        }

        public List<Employee> SearchByNamePart(string searchText)
        {
            var lower = searchText.ToLower();
            return _items.Where(e => 
                e.FirstName.ToLower().Contains(lower) || 
                e.LastName.ToLower().Contains(lower)).ToList();
        }

        public decimal GetAverageSalary()
        {
            if (_items.Count == 0) return 0;
            return _items.Sum(e => e.BaseSalary) / _items.Count;
        }

        public List<Employee> GetByHighestSalary()
        {
            if (_items.Count == 0) return new List<Employee>();
            var maxSalary = _items.Max(e => e.BaseSalary);
            return _items.Where(e => e.BaseSalary == maxSalary).ToList();
        }
    }
}

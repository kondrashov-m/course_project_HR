using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using HRSystem.Models;

namespace HRSystem.Repositories
{
    /// <summary>
    /// ����������� ��� �������� ����������� � ������.
    /// </summary>
    public class InMemoryRepository : IEmployeeRepository
    {
        private static readonly List<Employee> _employees = new List<Employee>();
        private static int _nextId = 1;
        private static readonly string _filePath = Path.Combine(AppContext.BaseDirectory, "employees.csv");

        static InMemoryRepository()
        {
            LoadFromFile();
        }

        private static void LoadFromFile()
        {
            if (!File.Exists(_filePath)) return;

            try
            {
                var lines = File.ReadAllLines(_filePath, new System.Text.UTF8Encoding(false));
                foreach (var line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;
                    var parts = line.Split(';');
                    // Expected: Id;First;Last;Position;Department;BaseSalary;HireDate;LastVacationDate
                    if (parts.Length < 6) continue;
                    var emp = new Employee
                    {
                        Id = int.TryParse(parts[0], out var idVal) ? idVal : 0,
                        FirstName = parts[1],
                        LastName = parts[2],
                        PositionName = parts[3],
                        DepartmentName = parts[4],
                        BaseSalary = decimal.TryParse(parts[5], NumberStyles.Any, CultureInfo.InvariantCulture, out var sal) ? sal : 0m,
                        HireDate = parts.Length > 6 && DateTime.TryParse(parts[6], out var hd) ? hd : DateTime.Now
                    };
                    _employees.Add(emp);
                }

                if (_employees.Count > 0)
                    _nextId = _employees.Max(e => e.Id) + 1;
            }
            catch
            {
                // ignore load errors
            }
        }

        private static void SaveToFile()
        {
            try
            {
                var lines = _employees.Select(e => string.Join(";", new string[] {
                    e.Id.ToString(),
                    e.FirstName ?? string.Empty,
                    e.LastName ?? string.Empty,
                    e.PositionName ?? string.Empty,
                    e.DepartmentName ?? string.Empty,
                    e.BaseSalary.ToString(CultureInfo.InvariantCulture),
                    e.HireDate.ToString("o")
                }));

                File.WriteAllLines(_filePath, lines, new System.Text.UTF8Encoding(false));
            }
            catch
            {
                // ignore save errors
            }
        }

        public List<Employee> GetAll() => _employees.ToList();

        public Employee GetById(int id) => _employees.FirstOrDefault(e => e.Id == id);

        public void Add(Employee employee)
        {
            employee.Id = _nextId++;
            _employees.Add(employee);
            SaveToFile();
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
                SaveToFile();
            }
        }

        public void Delete(int id)
        {
            var emp = _employees.FirstOrDefault(e => e.Id == id);
            if (emp != null)
            {
                _employees.Remove(emp);
                SaveToFile();
            }
        }

        public int GetNextId() => _nextId;
    }
}

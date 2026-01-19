using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using HRSystem.Models;

namespace HRSystem.Repositories
{
    /// <summary>
    /// Репозиторий для хранения сотрудников в JSON.
    /// </summary>
    public class InMemoryRepository : IEmployeeRepository
    {
        private static readonly List<Employee> _employees = new List<Employee>();
        private static readonly List<Salary> _salaries = new List<Salary>();
        private static readonly List<Vacation> _vacations = new List<Vacation>();
        private static int _nextEmployeeId = 1;
        private static int _nextSalaryId = 1;
        private static int _nextVacationId = 1;
        private static readonly string _dbFilePath = Path.Combine(AppContext.BaseDirectory, "hrdb.json");

        static InMemoryRepository()
        {
            LoadFromJson();
        }

        /// <summary>
        /// Модель для JSON сохранения всех данных
        /// </summary>
        [JsonSourceGenerationOptions(WriteIndented = true)]
        private class DatabaseModel
        {
            public List<Employee> Employees { get; set; } = new();
            public List<Salary> Salaries { get; set; } = new();
            public List<Vacation> Vacations { get; set; } = new();
            public int NextEmployeeId { get; set; } = 1;
            public int NextSalaryId { get; set; } = 1;
            public int NextVacationId { get; set; } = 1;
        }

        private static void LoadFromJson()
        {
            if (!File.Exists(_dbFilePath))
            {
                // Создаем пустую БД если файла нет
                var emptyDb = new DatabaseModel();
                SaveToJson();
                return;
            }

            try
            {
                var json = File.ReadAllText(_dbFilePath, new System.Text.UTF8Encoding(false));
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var db = JsonSerializer.Deserialize<DatabaseModel>(json, options);

                if (db != null)
                {
                    _employees.Clear();
                    _employees.AddRange(db.Employees ?? new List<Employee>());
                    
                    _salaries.Clear();
                    _salaries.AddRange(db.Salaries ?? new List<Salary>());
                    
                    _vacations.Clear();
                    _vacations.AddRange(db.Vacations ?? new List<Vacation>());
                    
                    _nextEmployeeId = db.NextEmployeeId;
                    _nextSalaryId = db.NextSalaryId;
                    _nextVacationId = db.NextVacationId;
                }
            }
            catch
            {
                // ignore load errors
            }
        }

        private static void SaveToJson()
        {
            try
            {
                var db = new DatabaseModel
                {
                    Employees = _employees.ToList(),
                    Salaries = _salaries.ToList(),
                    Vacations = _vacations.ToList(),
                    NextEmployeeId = _nextEmployeeId,
                    NextSalaryId = _nextSalaryId,
                    NextVacationId = _nextVacationId
                };

                var options = new JsonSerializerOptions { WriteIndented = true };
                var json = JsonSerializer.Serialize(db, options);
                File.WriteAllText(_dbFilePath, json, new System.Text.UTF8Encoding(false));
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
            employee.Id = _nextEmployeeId++;
            _employees.Add(employee);
            SaveToJson();
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
                SaveToJson();
            }
        }

        public void Delete(int id)
        {
            var emp = _employees.FirstOrDefault(e => e.Id == id);
            if (emp != null)
            {
                _employees.Remove(emp);
                SaveToJson();
            }
        }

        public int GetNextId() => _nextEmployeeId;

        // Salary methods
        public List<Salary> GetAllSalaries() => _salaries.ToList();

        public Salary GetSalaryById(int id) => _salaries.FirstOrDefault(s => s.Id == id);

        public void AddSalary(Salary salary)
        {
            salary.Id = _nextSalaryId++;
            _salaries.Add(salary);
            SaveToJson();
        }

        public void UpdateSalary(Salary salary)
        {
            var existing = _salaries.FirstOrDefault(s => s.Id == salary.Id);
            if (existing != null)
            {
                existing.EmployeeId = salary.EmployeeId;
                existing.BaseSalary = salary.BaseSalary;
                existing.Bonus = salary.Bonus;
                existing.Deductions = salary.Deductions;
                existing.TotalSalary = salary.TotalSalary;
                existing.PaymentDate = salary.PaymentDate;
                SaveToJson();
            }
        }

        public void DeleteSalary(int id)
        {
            var salary = _salaries.FirstOrDefault(s => s.Id == id);
            if (salary != null)
            {
                _salaries.Remove(salary);
                SaveToJson();
            }
        }

        public List<Salary> GetSalariesByEmployeeId(int employeeId) => _salaries.Where(s => s.EmployeeId == employeeId).ToList();

        // Vacation methods
        public List<Vacation> GetAllVacations() => _vacations.ToList();

        public Vacation GetVacationById(int id) => _vacations.FirstOrDefault(v => v.Id == id);

        public void AddVacation(Vacation vacation)
        {
            vacation.Id = _nextVacationId++;
            _vacations.Add(vacation);
            SaveToJson();
        }

        public void UpdateVacation(Vacation vacation)
        {
            var existing = _vacations.FirstOrDefault(v => v.Id == vacation.Id);
            if (existing != null)
            {
                existing.EmployeeId = vacation.EmployeeId;
                existing.StartDate = vacation.StartDate;
                existing.EndDate = vacation.EndDate;
                existing.Reason = vacation.Reason;
                existing.Status = vacation.Status;
                SaveToJson();
            }
        }

        public void DeleteVacation(int id)
        {
            var vacation = _vacations.FirstOrDefault(v => v.Id == id);
            if (vacation != null)
            {
                _vacations.Remove(vacation);
                SaveToJson();
            }
        }

        public List<Vacation> GetVacationsByEmployeeId(int employeeId) => _vacations.Where(v => v.EmployeeId == employeeId).ToList();
    }
}

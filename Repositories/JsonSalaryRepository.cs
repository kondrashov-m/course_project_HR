
using System.Text.Json;
using HRSystem.Models;

namespace HRSystem.Repositories
{
    public class JsonSalaryRepository
    {
        private readonly string _filePath = "salaries.json";
        private List<Salary> _salaries;
        
        public JsonSalaryRepository()
        {
            _salaries = LoadFromFile();
        }
        
        public void Add(Salary salary)
        {
            salary.Id = GetNextId();
            _salaries.Add(salary);
            SaveToFile();
        }
        
        public List<Salary> GetByEmployeeId(int employeeId)
        {
            return _salaries.Where(s => s.EmployeeId == employeeId).ToList();
        }
        
        public Salary GetByEmployeeAndDate(int employeeId, int month, int year)
        {
            return _salaries.FirstOrDefault(s => 
                s.EmployeeId == employeeId && 
                s.Month == month && 
                s.Year == year);
        }
        
        public List<Salary> GetAll()
        {
            return _salaries;
        }
        
        private int GetNextId()
        {
            return _salaries.Count > 0 ? _salaries.Max(s => s.Id) + 1 : 1;
        }
        
        private List<Salary> LoadFromFile()
        {
            if (File.Exists(_filePath))
            {
                var json = File.ReadAllText(_filePath);
                return JsonSerializer.Deserialize<List<Salary>>(json) ?? new List<Salary>();
            }
            return new List<Salary>();
        }
        
        private void SaveToFile()
        {
            var json = JsonSerializer.Serialize(_salaries, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, json);
        }
    }
}
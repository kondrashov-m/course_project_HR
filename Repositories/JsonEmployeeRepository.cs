using System.Text.Json;
using HRSystem.Models;

namespace HRSystem.Repositories
{
    public class JsonEmployeeRepository : IEmployeeRepository
    {
        private readonly string _filePath = "employees.json";
        private List<Employee> _employees;
        
        public JsonEmployeeRepository()
        {
            _employees = LoadFromFile();
        }
        
        public Employee Add(Employee employee)
        {
            employee.Id = GetNextId();
            _employees.Add(employee);
            SaveToFile();
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
            var index = _employees.FindIndex(e => e.Id == employee.Id);
            if (index >= 0)
            {
                _employees[index] = employee;
                SaveToFile();
            }
        }
        
        public bool Delete(int id)
        {
            var employee = GetById(id);
            if (employee != null)
            {
                _employees.Remove(employee);
                SaveToFile();
                return true;
            }
            return false;
        }
        
        private int GetNextId()
        {
            return _employees.Count > 0 ? _employees.Max(e => e.Id) + 1 : 1;
        }
        
        private List<Employee> LoadFromFile()
        {
            if (File.Exists(_filePath))
            {
                var json = File.ReadAllText(_filePath);
                return JsonSerializer.Deserialize<List<Employee>>(json) ?? new List<Employee>();
            }
            return new List<Employee>();
        }
        
        private void SaveToFile()
        {
            var json = JsonSerializer.Serialize(_employees, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, json);
        }
    }
}
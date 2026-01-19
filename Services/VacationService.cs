using System.Text.Json;
using HRSystem.Models;
using HRSystem.Repositories;

namespace HRSystem.Services
{
    public class VacationService : IVacationService
    {
        private List<Vacation> _vacations;
        private readonly IEmployeeRepository _employeeRepository;
        private int _nextId = 1;
        private readonly string _filePath = "vacations.json";
        
        public VacationService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
            _vacations = LoadFromFile();
            if (_vacations.Any()) _nextId = _vacations.Max(v => v.Id) + 1;
        }
        
        public Vacation CreateVacation(int employeeId, DateTime startDate, DateTime endDate)
        {
            var employee = _employeeRepository.GetById(employeeId);
            if (employee == null) throw new ArgumentException("Сотрудник не найден");
            
            var vacation = new Vacation
            {
                Id = _nextId++,
                EmployeeId = employeeId,
                StartDate = startDate,
                EndDate = endDate,
                Status = "Pending"
            };
            _vacations.Add(vacation);
            SaveToFile();
            return vacation;
        }
        
        public List<Vacation> GetEmployeeVacations(int employeeId)
        {
            return _vacations.Where(v => v.EmployeeId == employeeId).ToList();
        }
        
        public bool ApproveVacation(int vacationId)
        {
            var vacation = _vacations.FirstOrDefault(v => v.Id == vacationId);
            if (vacation != null)
            {
                vacation.Status = "Approved";
                SaveToFile();
                return true;
            }
            return false;
        }
        
        public bool RejectVacation(int vacationId)
        {
            var vacation = _vacations.FirstOrDefault(v => v.Id == vacationId);
            if (vacation != null)
            {
                vacation.Status = "Rejected";
                SaveToFile();
                return true;
            }
            return false;
        }
        
        private List<Vacation> LoadFromFile()
        {
            if (File.Exists(_filePath))
            {
                var json = File.ReadAllText(_filePath);
                return JsonSerializer.Deserialize<List<Vacation>>(json) ?? new List<Vacation>();
            }
            return new List<Vacation>();
        }
        
        private void SaveToFile()
        {
            var json = JsonSerializer.Serialize(_vacations, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, json);
        }
    }
}
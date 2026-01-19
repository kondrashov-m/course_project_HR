using System;

namespace HRSystem.Models
{
    public class Contract
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Type { get; set; }

        public Contract()
        {
            StartDate = DateTime.Now;
            Type = "Бессрочный";
        }

        public Contract(int employeeId, DateTime startDate, DateTime? endDate, string type)
        {
            EmployeeId = employeeId;
            StartDate = startDate;
            EndDate = endDate;
            Type = type;
        }

        public override string ToString()
        {
            return $"Тип: {Type}, Начало: {StartDate:dd.MM.yyyy}, Окончание: {EndDate?.ToString("dd.MM.yyyy") ?? "по настоящее время"}";
        }
    }
}

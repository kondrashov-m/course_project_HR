using System;

namespace HRSystem.Models
{
    /// <summary>
    /// Класс, представляющий трудовой договор.
    /// </summary>
    public class Contract
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Type { get; set; } // "Без срока", "Срочный", "Контракт"

        public Contract()
        {
            StartDate = DateTime.Now;
            Type = "Без срока";
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
            return $"Договор: {Type}, Начало: {StartDate:dd.MM.yyyy}, Конец: {EndDate?.ToString("dd.MM.yyyy") ?? "без срока"}";
        }
    }
}

using System;

namespace HRSystem.Models
{
    /// <summary>
    /// Класс, представляющий отпуск сотрудника.
    /// </summary>
    public class Vacation
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; } // "Утвержден", "В процессе", "Отменен"

        public Vacation()
        {
            StartDate = DateTime.Now;
            Status = "В процессе";
        }

        public Vacation(int employeeId, DateTime startDate, DateTime endDate, string status)
        {
            EmployeeId = employeeId;
            StartDate = startDate;
            EndDate = endDate;
            Status = status;
        }

        public override string ToString()
        {
            return $"Отпуск: {StartDate:dd.MM.yyyy} — {EndDate:dd.MM.yyyy} — Статус: {Status}";
        }
    }
}

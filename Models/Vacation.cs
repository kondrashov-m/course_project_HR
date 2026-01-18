using System;

namespace HRSystem.Models
{
    /// <summary>
    /// �����, �������������� ������ ����������.
    /// </summary>
    public class Vacation
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }        public string Reason { get; set; }        public string Status { get; set; } // "���������", "� ��������", "�������"

        public Vacation()
        {
            StartDate = DateTime.Now;
            Status = "� ��������";
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
            return $"������: {StartDate:dd.MM.yyyy} � {EndDate:dd.MM.yyyy} � ������: {Status}";
        }
    }
}

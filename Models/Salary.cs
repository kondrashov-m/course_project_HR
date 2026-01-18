using System;

namespace HRSystem.Models
{
    /// <summary>
    /// �����, �������������� �������� ����������.
    /// </summary>
    public class Salary
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public decimal BaseSalary { get; set; }
        public decimal Bonus { get; set; }
        public decimal Deductions { get; set; }
        public decimal TotalSalary { get; set; }
        public DateTime PaymentDate { get; set; }

        public Salary()
        {
            PaymentDate = DateTime.Now;
            Bonus = 0;
            Deductions = 0;
        }

        public Salary(decimal baseSalary, decimal bonus, decimal deductions = 0)
        {
            BaseSalary = baseSalary;
            Bonus = bonus;
            Deductions = deductions;
            TotalSalary = baseSalary + bonus - deductions;
            PaymentDate = DateTime.Now;
        }

        public void CalculateTotal()
        {
            TotalSalary = BaseSalary + Bonus;
        }

        public override string ToString()
        {
            return $"��������: {TotalSalary:F2} (����: {BaseSalary:F2}, ������: {Bonus:F2})";
        }
    }
}

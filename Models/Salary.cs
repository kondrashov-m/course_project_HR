using System;

namespace HRSystem.Models
{
    /// <summary>
    /// Класс, представляющий зарплату сотрудника.
    /// </summary>
    public class Salary
    {
        public int EmployeeId { get; set; }
        public decimal BaseSalary { get; set; }
        public decimal Bonus { get; set; }
        public decimal TotalSalary { get; set; }
        public DateTime PayDate { get; set; }

        public Salary()
        {
            PayDate = DateTime.Now;
        }

        public Salary(decimal baseSalary, decimal bonus)
        {
            BaseSalary = baseSalary;
            Bonus = bonus;
            TotalSalary = baseSalary + bonus;
            PayDate = DateTime.Now;
        }

        public void CalculateTotal()
        {
            TotalSalary = BaseSalary + Bonus;
        }

        public override string ToString()
        {
            return $"Зарплата: {TotalSalary:F2} (База: {BaseSalary:F2}, Премия: {Bonus:F2})";
        }
    }
}

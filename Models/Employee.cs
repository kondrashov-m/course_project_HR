using System;
using System.ComponentModel.DataAnnotations;

namespace HRSystem.Models
{
    /// <summary>
    /// Класс, представляющий сотрудника предприятия.
    /// </summary>
    public class Employee
    {
        // Свойства сотрудника
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PositionName { get; set; }
        public string DepartmentName { get; set; }
        public decimal BaseSalary { get; set; }
        public DateTime HireDate { get; set; }
        public DateTime? LastVacationDate { get; set; } // Дата последнего отпуска

        // Конструктор по умолчанию
        public Employee()
        {
            HireDate = DateTime.Now;
        }

        // Конструктор с параметрами
        public Employee(string firstName, string lastName, string positionName, string departmentName, decimal baseSalary)
        {
            FirstName = firstName;
            LastName = lastName;
            PositionName = positionName;
            DepartmentName = departmentName;
            BaseSalary = baseSalary;
            HireDate = DateTime.Now;
        }

        // Метод для форматированного вывода
        public override string ToString()
        {
            return $"{FirstName} {LastName} — {PositionName}, {DepartmentName}, Зарплата: {BaseSalary:F2}";
        }
    }
}

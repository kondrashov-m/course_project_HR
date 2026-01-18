using System;
using System.ComponentModel.DataAnnotations;

namespace HRSystem.Models
{
    /// <summary>
    /// �����, �������������� ���������� �����������.
    /// </summary>
    public class Employee
    {
        // �������� ����������
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PositionName { get; set; }
        public string DepartmentName { get; set; }
        public decimal BaseSalary { get; set; }
        public DateTime HireDate { get; set; }
        public DateTime? LastVacationDate { get; set; } // ���� ���������� �������

        // ����������� �� ���������
        public Employee()
        {
            HireDate = DateTime.Now;
        }

        // ����������� � �����������
        public Employee(string firstName, string lastName, string positionName, string departmentName, decimal baseSalary)
        {
            FirstName = (firstName ?? string.Empty).Trim();
            LastName = (lastName ?? string.Empty).Trim();
            PositionName = (positionName ?? string.Empty).Trim();
            DepartmentName = (departmentName ?? string.Empty).Trim();
            BaseSalary = baseSalary;
            HireDate = DateTime.Now;
        }

        // ����� ��� ���������������� ������
        public override string ToString()
        {
            return $"{FirstName} {LastName} � {PositionName}, {DepartmentName}, ��������: {BaseSalary:F2}";
        }
    }
}

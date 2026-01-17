using System.ComponentModel.DataAnnotations;

namespace HRSystem.Models
{
    /// <summary>
    ///  ласс, представл€ющий должность сотрудника.
    /// </summary>
    public class Position
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal AverageSalary { get; set; }

        public Position()
        {
            Id = 0;
            Name = "";
            AverageSalary = 0;
        }

        public Position(string name, decimal averageSalary)
        {
            Name = name;
            AverageSalary = averageSalary;
        }
    }
}

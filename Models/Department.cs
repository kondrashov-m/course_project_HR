using System;

namespace HRSystem.Models
{
    /// <summary>
    /// �����, �������������� ����� �����������.
    /// </summary>
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ManagerName { get; set; }

        public Department()
        {
            Id = 0;
            Name = "";
            ManagerName = "";
        }

        public Department(string name, string managerName)
        {
            Name = name;
            ManagerName = managerName;
        }

        public override string ToString()
        {
            return $"{Name} � ������������: {ManagerName}";
        }
    }
}

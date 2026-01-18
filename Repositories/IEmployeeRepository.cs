using System.Collections.Generic;using HRSystem.Models;
namespace HRSystem.Repositories
{
    /// <summary>
    /// ��������� ����������� ��� �����������.
    /// </summary>
    public interface IEmployeeRepository
    {
        List<Employee> GetAll();
        Employee GetById(int id);
        void Add(Employee employee);
        void Update(Employee employee);
        void Delete(int id);
        int GetNextId();
    }
}

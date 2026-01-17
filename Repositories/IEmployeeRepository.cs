using System.Collections.Generic;

namespace HRSystem.Repositories
{
    /// <summary>
    /// Интерфейс репозитория для сотрудников.
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

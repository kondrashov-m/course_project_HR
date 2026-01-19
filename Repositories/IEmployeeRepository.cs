using HRSystem.Models;

namespace HRSystem.Repositories
{
    public interface IEmployeeRepository
    {
        Employee Add(Employee employee);
        Employee GetById(int id);
        List<Employee> GetAll();
        void Update(Employee employee);
        bool Delete(int id);
    }
}
using System.Collections.Generic;
using HRSystem.Models;

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

        // Salary methods
        List<Salary> GetAllSalaries();
        Salary GetSalaryById(int id);
        void AddSalary(Salary salary);
        void UpdateSalary(Salary salary);
        void DeleteSalary(int id);
        List<Salary> GetSalariesByEmployeeId(int employeeId);

        // Vacation methods
        List<Vacation> GetAllVacations();
        Vacation GetVacationById(int id);
        void AddVacation(Vacation vacation);
        void UpdateVacation(Vacation vacation);
        void DeleteVacation(int id);
        List<Vacation> GetVacationsByEmployeeId(int employeeId);
    }
}

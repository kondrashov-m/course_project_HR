using System.Collections.Generic;
using HRSystem.Models;

namespace HRSystem.Services
{
    /// <summary>
    /// Интерфейс для управления отпусками.
    /// </summary>
    public interface IVacationService
    {
        List<Vacation> GetAllVacations();
        Vacation GetVacationById(int id);
        void AddVacation(Vacation vacation);
        void UpdateVacation(Vacation vacation);
        void DeleteVacation(int id);
        List<Vacation> GetVacationsByEmployee(int employeeId);
    }
}

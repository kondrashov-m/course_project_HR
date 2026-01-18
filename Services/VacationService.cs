using System;
using System.Collections.Generic;
using System.Linq;
using HRSystem.Models;
using HRSystem.Repositories;

namespace HRSystem.Services
{
    /// <summary>
    /// Сервис для управления отпусками.
    /// </summary>
    public class VacationService : IVacationService
    {
        private static List<Vacation> _vacations = new List<Vacation>();
        private static int _nextId = 1;

        public List<Vacation> GetAllVacations() => _vacations.ToList();

        public Vacation GetVacationById(int id) => _vacations.FirstOrDefault(v => v.Id == id);

        public void AddVacation(Vacation vacation)
        {
            vacation.Id = _nextId++;
            _vacations.Add(vacation);
        }

        public void UpdateVacation(Vacation vacation)
        {
            var existing = _vacations.FirstOrDefault(v => v.Id == vacation.Id);
            if (existing != null)
            {
                existing.EmployeeId = vacation.EmployeeId;
                existing.StartDate = vacation.StartDate;
                existing.EndDate = vacation.EndDate;
                existing.Reason = vacation.Reason;
            }
        }

        public void DeleteVacation(int id)
        {
            var vacation = _vacations.FirstOrDefault(v => v.Id == id);
            if (vacation != null)
                _vacations.Remove(vacation);
        }

        public List<Vacation> GetVacationsByEmployee(int employeeId)
        {
            return _vacations.Where(v => v.EmployeeId == employeeId).ToList();
        }
    }
}

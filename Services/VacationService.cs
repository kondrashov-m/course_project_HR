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
        private readonly IEmployeeRepository _repository;

        public VacationService(IEmployeeRepository repository)
        {
            _repository = repository;
        }

        public List<Vacation> GetAllVacations() => _repository.GetAllVacations();

        public Vacation GetVacationById(int id) => _repository.GetVacationById(id);

        public void AddVacation(Vacation vacation)
        {
            _repository.AddVacation(vacation);
        }

        public void UpdateVacation(Vacation vacation)
        {
            _repository.UpdateVacation(vacation);
        }

        public void DeleteVacation(int id)
        {
            _repository.DeleteVacation(id);
        }

        public List<Vacation> GetVacationsByEmployee(int employeeId)
        {
            return _repository.GetVacationsByEmployeeId(employeeId);
        }
    }
}

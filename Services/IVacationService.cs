using HRSystem.Models;

namespace HRSystem.Services
{
    public interface IVacationService
    {
        Vacation CreateVacation(int employeeId, DateTime startDate, DateTime endDate);
        List<Vacation> GetEmployeeVacations(int employeeId);
        bool ApproveVacation(int vacationId);
        bool RejectVacation(int vacationId);
    }
}
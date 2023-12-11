using CodeChallenge.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CodeChallenge.Repositories
{
    public interface ICompensationRepository
    {
        Compensation GetById(string employeeId);
        Compensation GetMostRecentByEmployeeId(string employeeId);
        List<Compensation> GetHistoryForEmployeeId(string employeeId);
        Compensation Add(Compensation compensation);
        Task SaveAsync();
    }
}
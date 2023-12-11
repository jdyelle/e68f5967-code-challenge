using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeChallenge.Models;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using CodeChallenge.Data;

namespace CodeChallenge.Repositories
{
    // TODO: Challenge-3
    public class CompensationRespository : ICompensationRepository
    {
        private readonly CompensationContext _compensationContext;
        private readonly ILogger<ICompensationRepository> _logger;

        public CompensationRespository(ILogger<ICompensationRepository> logger, CompensationContext compensationContext)
        {
            _compensationContext = compensationContext;
            _logger = logger;
        }

        public Compensation Add(Compensation compensation)
        {
            compensation.CompensationId = Guid.NewGuid().ToString();
            _compensationContext.Compensations.Add(compensation);
            return compensation;
        }
        public Compensation GetById(string id)
        {
            return _compensationContext.Compensations.Where(c => c.CompensationId == id).FirstOrDefault();
        }

        public Compensation GetMostRecentByEmployeeId(string employeeId)
        {
            return _compensationContext.Compensations.Where(c => c.EmployeeId == employeeId).OrderByDescending(c => c.EffectiveDate).FirstOrDefault();
        }

        //Bonus function (since the functionality was close anyway) :)
        public List<Compensation> GetHistoryForEmployeeId(string employeeId)
        {
            return _compensationContext.Compensations.Where(c => c.EmployeeId == employeeId).OrderByDescending(c => c.EffectiveDate).ToList<Compensation>();
        }

        public Task SaveAsync()
        {
            return _compensationContext.SaveChangesAsync();
        }
    }
}

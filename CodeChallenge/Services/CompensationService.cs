using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeChallenge.Models;
using Microsoft.Extensions.Logging;
using CodeChallenge.Repositories;

namespace CodeChallenge.Services
{
    public class CompensationService : ICompensationService
    {
        private readonly ICompensationRepository _compensationRepository;
        private readonly IEmployeeService _employeeService;
        private readonly ILogger<CompensationService> _logger;

        public CompensationService(ILogger<CompensationService> logger, ICompensationRepository compensationRepository, IEmployeeService employeeService)
        {
            _compensationRepository = compensationRepository;
            _employeeService = employeeService;
            _logger = logger;
        }

        public Compensation Create(Compensation compensation)
        {
            if (_employeeService.GetById(compensation.EmployeeId) == null) throw new ArgumentException("Requested EmployeeId does not exist");
            if(compensation != null)
            {
                _compensationRepository.Add(compensation);
                _compensationRepository.SaveAsync().Wait();
            }

            return compensation;
        }

        public Compensation GetById(string id)
        {
            if(!String.IsNullOrEmpty(id))
            {
                return _compensationRepository.GetById(id);
            }

            return null;
        }

        public Compensation GetMostRecentByEmployeeId(string employeeId)
        {
            if (!String.IsNullOrEmpty(employeeId))
            {
                return _compensationRepository.GetMostRecentByEmployeeId(employeeId);
            }

            return null;
        }

        public List<Compensation> GetHistoryForEmployeeId(string employeeId)
        {
            if (!String.IsNullOrEmpty(employeeId))
            {
                return _compensationRepository.GetHistoryForEmployeeId(employeeId);
            }

            return null;
        }
    }
}

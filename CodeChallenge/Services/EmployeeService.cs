using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeChallenge.Models;
using Microsoft.Extensions.Logging;
using CodeChallenge.Repositories;

namespace CodeChallenge.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<EmployeeService> _logger;

        public EmployeeService(ILogger<EmployeeService> logger, IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
            _logger = logger;
        }

        public Employee Create(Employee employee)
        {
            if(employee != null)
            {
                _employeeRepository.Add(employee);
                _employeeRepository.SaveAsync().Wait();
            }

            return employee;
        }

        public Employee GetById(string id)
        {
            if(!String.IsNullOrEmpty(id))
            {
                return _employeeRepository.GetById(id);
            }

            return null;
        }

        public Employee Replace(Employee originalEmployee, Employee newEmployee)
        {
            if(originalEmployee != null)
            {
                _employeeRepository.Remove(originalEmployee);
                if (newEmployee != null)
                {
                    // ensure the original has been removed, otherwise EF will complain another entity w/ same id already exists
                    _employeeRepository.SaveAsync().Wait();

                    _employeeRepository.Add(newEmployee);
                    // overwrite the new id with previous employee id
                    newEmployee.EmployeeId = originalEmployee.EmployeeId;
                }
                _employeeRepository.SaveAsync().Wait();
            }

            return newEmployee;
        }

        // TODO: Challenge-2
        /* This probably goes without saying but as much of the non-validation business logic goes into the service 
         * as possible to help with testing and such.
         */
        public ReportingStructure GetEmployeeReportingStructure(Employee root)
        {
            // We're going to use this as a basic form of loop detection :)
            Dictionary<string, Employee> _employeeReports = new Dictionary<string, Employee>();
            PopulateSubTree(root, _employeeReports);

            return new ReportingStructure { employee = root, numberOfReports = _employeeReports.Count };
        }
        
        private void PopulateSubTree(Employee employee, Dictionary<string, Employee> _employeeReports)
        {
            foreach (Employee directReport in employee.DirectReports)
            {
                if (!_employeeReports.ContainsKey(directReport.EmployeeId)) //If the employee has already been indexed, don't re-index
                {                    
                    Employee report = GetById(directReport.EmployeeId); //Re-fetch the next level of this same report to get its reports
                    _employeeReports.Add(directReport.EmployeeId, directReport); // Add to the dictionary as a report

                    Employee currentReport = employee.DirectReports.Find(employee => employee.EmployeeId == report.EmployeeId);
                    currentReport = report; // Update the original node of the graph with the node that has subreports fetched from EF

                    PopulateSubTree(report, _employeeReports); // Recurse on this exact function in case there are nested reports again
                }
            }
        }
    }
}

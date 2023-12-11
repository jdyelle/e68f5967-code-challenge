using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CodeChallenge.Services;
using CodeChallenge.Models;

namespace CodeChallenge.Controllers
{
    [ApiController]
    [Route("api/compensation")]
    [Route("api/employee/{employeeId}/compensation")]
    public class CompensationController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly ICompensationService _compensationService;

        public CompensationController(ILogger<CompensationController> logger, ICompensationService compensationService)
        {
            _logger = logger;
            _compensationService = compensationService;
        }

        [HttpPost]
        public IActionResult CreateCompensation([FromBody] Compensation compensation, string? employeeId)
        {
            _logger.LogDebug($"Received compensation create request for '{compensation.EmployeeId}'");

            // Quick check to make sure we're not passing a compensation that doesn't belong to the employee (on the employee route).
            string _employeeId = employeeId ?? compensation.EmployeeId;
            if (_employeeId != compensation.EmployeeId) return Forbid(); 

            _compensationService.Create(compensation);
            return CreatedAtAction(nameof(GetCompensationById), new { id = compensation.CompensationId }, compensation);
        }

        /* This is mostly for fun -- but, there are different schools of thought about whether compensation should 
         * be a filter on a list of all compensations, or a subproperty (and therefore subroute) of an employee.
         * For this example, I've enabled two routes:
         * [/api/employee/{employeeId}/compensation] AND
         * [/api/compensation?employeeId={employeeId}] 
         * The reason for this is that they might have different persistence access patterns based on which system 
         * they're coming from -- and this is all easy when we're doing it in one app, but if we had an API gateway, 
         * it might be better to have the compensation endpoints broken out from the employee endpoints depending on
         * the upstream services or persistence.
         */

        [HttpGet]
        public IActionResult GetCompensationByEmployeeId(string? employeeId)
        {
            employeeId = employeeId ?? Request.Query["employeeId"];
            _logger.LogDebug($"Received compensation get request for '{employeeId}'");
            if (employeeId == null) return NotFound();   // This could just as easily return the list of all compensations, but
                                                         // we should check with the product manager because that's a risky endpoint.

            var compensation = _compensationService.GetMostRecentByEmployeeId(employeeId);
            if (compensation == null) return NotFound();
            return Ok(compensation);
        }

        // I'm leaving out an endpoint for the "Salary History" that I have in the service since it wasn't requested.

        [HttpGet("{id}")]
        public IActionResult GetCompensationById(string id)
        {
            _logger.LogDebug($"Received compensation get request for '{id}'");

            var compensation = _compensationService.GetById(id);

            if (compensation == null)
                return NotFound();

            return Ok(compensation);
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using challenge.Services;
using challenge.Models;

namespace challenge.Controllers
{
    [Route("api/compensation")]
    public class CompensationController : Controller
    {
        private readonly ILogger _logger;
        private readonly ICompensationService _compensationService;

        //Create the Controller
        public CompensationController(ILogger<CompensationController> logger, ICompensationService compensationService)
        {
            _logger = logger;
            _compensationService = compensationService;
        }

        //Post method to create a compensation object and store it based off the body of the object
        [HttpPost]
        public IActionResult CreateCompensation([FromBody] Compensation compensation)
        {
            _logger.LogDebug($"Received compensation create request for '{compensation.Employee.FirstName} {compensation.Employee.LastName}' with salary: '{compensation.Salary}' and effective date: '{compensation.EffectiveDate}'");

            _compensationService.Create(compensation);

            return CreatedAtRoute("getCompensationById", new { id = compensation.Employee.EmployeeId }, compensation);
        }
        //Get method for a compensation object that was stored in the database based on the query id
        [HttpGet("{id}", Name = "getCompensationByID")]
        public IActionResult GetCompensationByID(String id)
        {
            _logger.LogDebug($"Received compensation get request for employee: '{id}'");


            Compensation compensation = _compensationService.GetById(id);

            if (compensation == null)
                return NotFound();

            return Ok(compensation);
        }
    }
}

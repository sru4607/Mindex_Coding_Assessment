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
    [Route("api/reportingStructure")]
    public class ReportingStructureController : Controller
    {
        private readonly ILogger _logger;
        private readonly IReportingStructureService _reportingStructureService;

        public ReportingStructureController(ILogger<ReportingStructureController> logger, IReportingStructureService reportingStructureService)
        {
            _logger = logger;
            _reportingStructureService = reportingStructureService;
        }

        //Get request for a ReportingStructure based of query ID
        [HttpGet("{id}", Name = "getReportsByID")]
        public IActionResult GetReportsById(String id)
        {
            _logger.LogDebug($"Received report get request for '{id}'");

            ReportingStructure reports = _reportingStructureService.GetReportsById(id);

            if (reports == null)
                return NotFound();

            return Ok(reports);
        }

    }
}

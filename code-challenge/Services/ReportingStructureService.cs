using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using challenge.Models;
using Microsoft.Extensions.Logging;
using challenge.Repositories;

namespace challenge.Services
{
    public class ReportingStructureService : IReportingStructureService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<ReportingStructureService> _logger;

        public ReportingStructureService(ILogger<ReportingStructureService> logger, IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
            _logger = logger;
        }

        public ReportingStructure GetReportsById(string id)
        {
            if(!String.IsNullOrEmpty(id))
            {
                ReportingStructure createdReport = new ReportingStructure();
                //Get the employee based on the id
                createdReport.Employee = _employeeRepository.GetById(id);
                //If the employee is null no reporting structure can be generate return not found
                if (createdReport.Employee == null)
                {
                    return null;
                }
                //Get the number of reports using the function below
                createdReport.NumberOfReports = GetDirectReportCount(createdReport.Employee);

                return createdReport;
            }

            //No id return not found
            return null;
        }

        //Function to generate the amount of direct reports an employee has
        private int GetDirectReportCount(Employee employee)
        {
            //If there are no direct reports return 0
            if (employee.DirectReports == null)
            {
                return 0;
            }

            int count = employee.DirectReports.Count;
            //Call this method on all descendents
            foreach (Employee e in employee.DirectReports)
            {
                count += GetDirectReportCount(_employeeRepository.GetById(e.EmployeeId));
            }

            return count;

        }
    }
}

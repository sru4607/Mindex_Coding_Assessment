using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using challenge.Models;
using Microsoft.Extensions.Logging;
using challenge.Repositories;

namespace challenge.Services
{
    public class CompensationService : ICompensationService
    {
        private readonly ICompensationRepository _compensationRepository;
        private readonly ILogger<CompensationService> _logger;

        public CompensationService(ILogger<CompensationService> logger, ICompensationRepository compensationRepository)
        {
            _compensationRepository = compensationRepository;
            _logger = logger;
        }
        //Creates a compensation and saves it to the database
        public Compensation Create(Compensation compensation)
        {
            if(compensation != null)
            {
                _compensationRepository.CreateCompensation(compensation);
                _compensationRepository.SaveAsync().Wait();
            }

            return compensation;
        }
        //Returns a compensation based on the employee id
        public Compensation GetById(string id)
        {
            if(!String.IsNullOrEmpty(id))
            {
                return _compensationRepository.GetCompensationById(id);
            }

            return null;
        }
    }
}

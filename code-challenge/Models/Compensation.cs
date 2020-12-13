using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace challenge.Models
{
    public class Compensation
    {   
        public string CompensationId { get; set; }
        public Employee Employee { get; set; }
        public float Salary { get; set; }
        public DateTime EffectiveDate { get; set; }
        
    }
}

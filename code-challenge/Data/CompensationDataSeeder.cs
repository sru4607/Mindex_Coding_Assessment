using challenge.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace challenge.Data
{
    public class CompensationDataSeeder
    {
        //Context and Date File
        private CompensationContext _compensationContext;
        private const String COMPENSATION_SEED_DATA_FILE = "resources/CompensationSeedData.json";

        public CompensationDataSeeder(CompensationContext compensationContext)
        {
            _compensationContext = compensationContext;
        }

        //Seeds data as necessary
        public async Task Seed()
        {
            //If there is no data seed the data
            if(!_compensationContext.Compensations.Any())
            {
                //Load the data
                List<Compensation> compensations = LoadCompensations();
                //Add data
                _compensationContext.Compensations.AddRange(compensations);
                //Save changes
                await _compensationContext.SaveChangesAsync();
            }
        }

        //Load the data into a list 
        private List<Compensation> LoadCompensations()
        {
            //Open the file
            using (FileStream fs = new FileStream(COMPENSATION_SEED_DATA_FILE, FileMode.Open))
            {
                //Create a reader
                StreamReader reader = new StreamReader(fs);

                //Generate a string from the reader
                string info = reader.ReadToEnd();

                //Deserialize it into a dynamic array 
                dynamic array = JsonConvert.DeserializeObject(info);

                //List to fill and return
                List<Compensation> compensation = new List<Compensation>();

                //Process each compensation in the dynamic array
                foreach (var item in array)
                {
                    //Compensation and Employee to use
                    Compensation comp = new Compensation();
                    Employee newEmployee = new Employee();

                    //Set properties
                    dynamic employee = item.employee;

                    newEmployee.EmployeeId = employee.employeeId;
                    newEmployee.FirstName = employee.firstName;
                    newEmployee.LastName = employee.lastName;
                    newEmployee.Position = employee.position;
                    newEmployee.Department = employee.department;

                    comp.CompensationId = item.compensationId;
                    comp.EffectiveDate = item.effectiveDate;
                    comp.Salary = item.salary;
                    comp.Employee = newEmployee;

                    //Add to array
                    compensation.Add(comp);
                    
                }

                return compensation;

            }

        }
    }
}

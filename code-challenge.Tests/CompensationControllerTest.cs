using challenge.Controllers;
using challenge.Data;
using challenge.Models;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using code_challenge.Tests.Integration.Extensions;

using System;
using System.IO;
using System.Net;
using System.Net.Http;
using code_challenge.Tests.Integration.Helpers;
using System.Text;

using Newtonsoft.Json;
using System.Collections.Generic;

namespace code_challenge.Tests.Integration
{
    [TestClass]
    public class CompensationControllerTest
    {
        private static HttpClient _httpClient;
        private static TestServer _testServer;

        [ClassInitialize]
        public static void InitializeClass(TestContext context)
        {
            _testServer = new TestServer(WebHost.CreateDefaultBuilder()
                .UseStartup<TestServerStartup>()
                .UseEnvironment("Development"));

            _httpClient = _testServer.CreateClient();
        }

        [ClassCleanup]
        public static void CleanUpTest()
        {
            _httpClient.Dispose();
            _testServer.Dispose();
        }

        [TestMethod]
        public void CreateCompensation_Returns_Created()
        {
            // Arrange
            Compensation compensation = new Compensation()
            {
                Employee = new Employee()
                {
                    EmployeeId = Guid.NewGuid().ToString(),
                    FirstName = "Sam",
                    LastName = "Unferth",
                    Position = "Software Engineer",
                    Department = "Engineering",
                },
                Salary = 80000.10f,
                EffectiveDate = new DateTime(2021, 01, 01)
            };

            string requestContent = new JsonSerialization().ToJson(compensation);

            // Execute
            var postRequestTask = _httpClient.PostAsync("api/compensation",
               new StringContent(requestContent, Encoding.UTF8, "application/json"));
            var response = postRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

            Compensation newCompensation = response.DeserializeContent<Compensation>();
            Assert.AreEqual(compensation.Employee.EmployeeId, newCompensation.Employee.EmployeeId);
            Assert.AreEqual(compensation.Employee.FirstName, newCompensation.Employee.FirstName);
            Assert.AreEqual(compensation.Employee.LastName, newCompensation.Employee.LastName);
            Assert.AreEqual(compensation.Employee.Position, newCompensation.Employee.Position);
            Assert.AreEqual(compensation.Employee.Department, newCompensation.Employee.Department);
            Assert.AreEqual(compensation.Salary, newCompensation.Salary);
            Assert.AreEqual(compensation.EffectiveDate, newCompensation.EffectiveDate);
        }

        [TestMethod]
        public void GetEmployeeById_Returns_Ok()
        {
            // Arrange
            String employeeId = "Compensation-Test";
            String expectedFirstName = "J";
            String expectedLastName = "Lenny";
            String expectedPosition = "Development Manager";
            String expectedDepartment = "Engineering";
            float expectedSalary = 80000.10f;
            DateTime expectedEffectiveDate = new DateTime(2020, 12, 12);

            // Execute
            var getRequestTask = _httpClient.GetAsync($"api/compensation/{employeeId}");
            var response = getRequestTask.Result;


            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            String responseString = response.Content.ReadAsStringAsync().Result;
            dynamic responseCompensation = JsonConvert.DeserializeObject(responseString);
            DateTime responseDate = responseCompensation.effectiveDate;

            Assert.AreEqual(expectedFirstName, responseCompensation.employee.firstName.ToObject<String>());
            Assert.AreEqual(expectedLastName, responseCompensation.employee.lastName.ToObject<String>());
            Assert.AreEqual(expectedPosition, responseCompensation.employee.position.ToObject<String>());
            Assert.AreEqual(expectedDepartment, responseCompensation.employee.department.ToObject<String>());
            Assert.AreEqual(expectedSalary, responseCompensation.salary.ToObject<float>());
            Assert.AreEqual(expectedEffectiveDate, responseDate);

        }

        [TestMethod]
        public void UpdateEmployee_Returns_NotFound()
        {
            // Arrange
            string employeeId = "Invalid";

            // Execute
            var getRequestTask = _httpClient.GetAsync($"api/compensation/{employeeId}");
            var response = getRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}

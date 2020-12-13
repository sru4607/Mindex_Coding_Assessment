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

namespace code_challenge.Tests.Integration
{
    [TestClass]
    public class ReportingStructureControllerTest
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
        //Tests Get Reports by ID with at multiple levels of direct reports
        [TestMethod]
        public void GetReportsById_Returns_Ok_Trunk()
        {
            // Arrange
            string employeeId = "16a596ae-edd3-4847-99fe-c4518e82c86f";
            int expectedReports = 4;

            // Execute
            var getRequestTask = _httpClient.GetAsync($"api/reportingStructure/{employeeId}");
            var response = getRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            ReportingStructure reportingStructure = response.DeserializeContent<ReportingStructure>();
            Assert.AreEqual(expectedReports, reportingStructure.NumberOfReports);
        }
        [TestMethod]
        //Tests Get Reports by ID with 0 direct reports
        public void GetReportsById_Returns_Ok_Leaf()
        {
            // Arrange
            string employeeId = "c0c2293d-16bd-4603-8e08-638a9d18b22c";
            int expectedReports = 0;

            // Execute
            var getRequestTask = _httpClient.GetAsync($"api/reportingStructure/{employeeId}");
            var response = getRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            ReportingStructure reportingStructure = response.DeserializeContent<ReportingStructure>();
            Assert.AreEqual(expectedReports, reportingStructure.NumberOfReports);
        }
        //Tests Get Reports by ID with an invalid ID
        [TestMethod]
        public void GetReportsById_Returns_NotFound()
        {
            // Arrange
            string employeeId = "Invalid_ID";

            // Execute
            var getRequestTask = _httpClient.GetAsync($"api/reportingStructure/{employeeId}");
            var response = getRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }


    }
}

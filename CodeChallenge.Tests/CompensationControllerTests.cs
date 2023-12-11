
using System;
using System.Net;
using System.Net.Http;
using System.Text;

using CodeChallenge.Models;

using CodeCodeChallenge.Tests.Integration.Extensions;
using CodeCodeChallenge.Tests.Integration.Helpers;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CodeCodeChallenge.Tests.Integration
{
    [TestClass]
    public class CompensationControllerTests
    {
        private static HttpClient _httpClient;
        private static TestServer _testServer;

        [ClassInitialize]
        // Attribute ClassInitialize requires this signature
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
        public static void InitializeClass(TestContext context)
        {
            _testServer = new TestServer();
            _httpClient = _testServer.NewClient();
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
            var compensation = new Compensation()
            {
                EmployeeId = "16a596ae-edd3-4847-99fe-c4518e82c86f",
                Salary = 123456,
                EffectiveDate = System.DateTime.Parse("2023-12-10T20:16:40")
            };

            var requestContent = new JsonSerialization().ToJson(compensation);

            // Execute
            var postRequestTask = _httpClient.PostAsync("api/compensation",
               new StringContent(requestContent, Encoding.UTF8, "application/json"));
            var response = postRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

            var newCompensation = response.DeserializeContent<Compensation>();
            Assert.IsNotNull(newCompensation.CompensationId);
            Assert.AreEqual(compensation.EmployeeId, newCompensation.EmployeeId);
            Assert.AreEqual(compensation.Salary, newCompensation.Salary);
            Assert.AreEqual(compensation.EffectiveDate, newCompensation.EffectiveDate);
        }


        [TestMethod]
        public void GetCompensationById_Returns_Ok()
        {
            // Set Up
            var compensation = new Compensation()
            {
                EmployeeId = "16a596ae-edd3-4847-99fe-c4518e82c86f",
                Salary = 123456,
                EffectiveDate = System.DateTime.Parse("2023-12-10T20:16:40")
            };
            var requestContent = new JsonSerialization().ToJson(compensation);
            var postRequestTask = _httpClient.PostAsync("api/compensation",
               new StringContent(requestContent, Encoding.UTF8, "application/json"));
            var postResponse = postRequestTask.Result;
            var postCompensation = postResponse.DeserializeContent<Compensation>();

            // Execute
            var getRequestTask = _httpClient.GetAsync($"api/compensation/{postCompensation.CompensationId}");
            var getResponse = getRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, getResponse.StatusCode);
            var getCompensation = getResponse.DeserializeContent<Compensation>();
            Assert.AreEqual(getCompensation.EmployeeId, compensation.EmployeeId);
            Assert.AreEqual(getCompensation.Salary, compensation.Salary);
            Assert.AreEqual(getCompensation.EffectiveDate, compensation.EffectiveDate);

            /* Technically we could just compare the postCompensation with compensation since the POST
             * call is using CreatedAtAction, but I wanted to be super explicit about my checks existing
             * in the database.
             */
        }

        [TestMethod]
        public void GetCompensationByEmpId1_Returns_Ok()
        {
            // Arrange
            var employeeId = "16a596ae-edd3-4847-99fe-c4518e82c86f";
            var expectedSalary = 123456;
            var expectedEffectiveDate = System.DateTime.Parse("2023-12-10T20:16:40");

            // Execute
            var getRequestTask = _httpClient.GetAsync($"api/compensation?employeeId={employeeId}");
            var response = getRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var compensation = response.DeserializeContent<Compensation>();
            Assert.AreEqual(employeeId, compensation.EmployeeId);
            Assert.AreEqual(expectedSalary, compensation.Salary);
            Assert.AreEqual(expectedEffectiveDate, compensation.EffectiveDate);
        }

        [TestMethod]
        public void GetCompensationByEmpId2_Returns_Ok()
        {
            // Arrange
            var employeeId = "16a596ae-edd3-4847-99fe-c4518e82c86f";
            var expectedSalary = 123456;
            var expectedEffectiveDate = System.DateTime.Parse("2023-12-10T20:16:40");

            // Execute
            var getRequestTask = _httpClient.GetAsync($"api/employee/{employeeId}/compensation");
            var response = getRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var compensation = response.DeserializeContent<Compensation>();
            Assert.AreEqual(employeeId, compensation.EmployeeId);
            Assert.AreEqual(expectedSalary, compensation.Salary);
            Assert.AreEqual(expectedEffectiveDate, compensation.EffectiveDate);
        }

        [TestMethod]
        public void CompensationList_Returns_NotFound()
        {
            // Execute
            var getRequestTask = _httpClient.GetAsync($"api/compensation");
            var response = getRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

    }
}

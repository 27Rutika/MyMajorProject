using EInsurance.Controllers;
using EInsurance.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace EInsurance.xUnitTestProject
{
    public partial class InsurancesApiTests
    {
        [Fact]
        public void GetInsurances_OkResult()
        {
            // ARRANGE
            var dbName = nameof(InsurancesApiTests.GetInsurances_OkResult);
            var logger = Mock.Of<ILogger<InsurancesController>>();
            var dbContext = DbContextMocker.GetApplicationDbContext(dbName);
            var apiController = new InsurancesController(dbContext, logger);

            // ACT
            IActionResult actionResult = apiController.GetInsurances().Result;

            // ASSERT
            // --- Check if the ActionResult is of the type OkObjectResult
            Assert.IsType<OkObjectResult>(actionResult);

            // --- Check if the HTTP Response Code is 200 "Ok"
            int expectedStatusCode = (int)System.Net.HttpStatusCode.OK;
            int actualStatusCode = (actionResult as OkObjectResult).StatusCode.Value;
            Assert.Equal<int>(expectedStatusCode, actualStatusCode);
        }

        [Fact]
        public void GetInsurances_CheckCorrectResult()
        {
            // ARRANGE
            var dbName = nameof(InsurancesApiTests.GetInsurances_OkResult);
            var logger = Mock.Of<ILogger<InsurancesController>>();
            var dbContext = DbContextMocker.GetApplicationDbContext(dbName);
            var apiController = new InsurancesController(dbContext, logger);

            // ACT: Call the API action method
            IActionResult actionResult = apiController.GetInsurances().Result;

            // ASSERT: Check if the ActionResult is of the type OkObjectResult
            Assert.IsType<OkObjectResult>(actionResult);


            // ACT: Extract the OkResult result 
            var okResult = actionResult.Should().BeOfType<OkObjectResult>().Subject;

            // ASSERT: if the OkResult contains the object of the Correct Type
            Assert.IsAssignableFrom<List<Insurance>>(okResult.Value);


            // ACT: Extract the Categories from the result of the action
            var insurancesFromApi = okResult.Value.Should().BeAssignableTo<List<Insurance>>().Subject;

            // ASSERT: if the categories is NOT NULL
            Assert.NotNull(insurancesFromApi);

            // ASSERT: if the number of categories in the DbContext seed data
            //         is the same as the number of categories returned in the API Result
            Assert.Equal<int>(expected: DbContextMocker.TestData_Insurances.Length,
                              actual: insurancesFromApi.Count);

            // ASSERT: Test the data received from the API against the Seed Data
            int ndx = 0;
            foreach (Insurance insurance in DbContextMocker.TestData_Insurances)
            {
                // ASSERT: check if the Category ID is correct
                Assert.Equal<int>(expected: insurance.InsuranceId,
                                  actual: insurancesFromApi[ndx].InsuranceId);

                // ASSERT: check if the Category Name is correct
                Assert.Equal(expected: insurance.InsuranceName,
                             actual: insurancesFromApi[ndx].InsuranceName);

                _testOutputHelper.WriteLine($"Compared Row # {ndx} successfully");

                ndx++;          // now compare against the next element in the array
            }
        }
    }
}

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
        public void InsertInsurance_OkResult()
        {
            // ARRANGE
            var dbName = nameof(InsurancesApiTests.InsertInsurance_OkResult);
            var logger = Mock.Of<ILogger<InsurancesController>>();
            using var dbContext = DbContextMocker.GetApplicationDbContext(dbName);      // Disposable!
            var apiController = new InsurancesController(dbContext, logger);
            Insurance insuranceToAdd = new Insurance
            {
                InsuranceId = 5,
                InsuranceName = "New Insurance"             // IF = null, then: INVALID!  CategoryName is REQUIRED
            };

            // ACT
            IActionResult actionResultPost = apiController.PostInsurance(insuranceToAdd).Result;

            // ASSERT - check if the IActionResult is Ok
            Assert.IsType<OkObjectResult>(actionResultPost);

            // ASSERT - check if the Status Code is (HTTP 200) "Ok", (HTTP 201 "Created")
            int expectedStatusCode = (int)System.Net.HttpStatusCode.OK;
            var actualStatusCode = (actionResultPost as OkObjectResult).StatusCode.Value;
            Assert.Equal<int>(expectedStatusCode, actualStatusCode);

            // Extract the result from the IActionResult object.
            var postResult = actionResultPost.Should().BeOfType<OkObjectResult>().Subject;

            // ASSERT - if the result is a CreatedAtActionResult
            Assert.IsType<CreatedAtActionResult>(postResult.Value);

            // Extract the inserted Category object
            Insurance actualInsurance = (postResult.Value as CreatedAtActionResult).Value
                                      .Should().BeAssignableTo<Insurance>().Subject;

            // ASSERT - if the inserted Category object is NOT NULL
            Assert.NotNull(actualInsurance);

            Assert.Equal(insuranceToAdd.InsuranceId, actualInsurance.InsuranceId);
            Assert.Equal(insuranceToAdd.InsuranceName, actualInsurance.InsuranceName);
        }
    }
}

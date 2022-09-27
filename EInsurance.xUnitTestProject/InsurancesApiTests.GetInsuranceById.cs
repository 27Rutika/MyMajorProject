using EInsurance.Controllers;
using EInsurance.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace EInsurance.xUnitTestProject
{
    public partial class InsurancesApiTests
    {
        [Fact]
        public void GetInsuranceById_NotFoundResult()
        {
            // ARRANGE
            var dbName = nameof(InsurancesApiTests.GetInsuranceById_NotFoundResult);
            var logger = Mock.Of<ILogger<InsurancesController>>();

            // using (var db = DbContextMocker.GetApplicationDbContext(dbName))
            // {
            // }           // db.Dispose(); implicitly called when you exit the USING Block

            using var dbContext = DbContextMocker.GetApplicationDbContext(dbName);      // Disposable!
            var apiController = new InsurancesController(dbContext, logger);
            int findCategoryID = 900;

            // ACT
            IActionResult actionResultGet = apiController.GetInsurance(findCategoryID).Result;

            // ASSERT - check if the IActionResult is NotFound 
            Assert.IsType<NotFoundResult>(actionResultGet);

            // ASSERT - check if the Status Code is (HTTP 404) "NotFound"
            int expectedStatusCode = (int)System.Net.HttpStatusCode.NotFound;
            var actualStatusCode = (actionResultGet as NotFoundResult).StatusCode;
            Assert.Equal<int>(expectedStatusCode, actualStatusCode);
        }

        [Fact]
        public void GetInsuranceById_BadRequestResult()
        {
            // ARRANGE
            var dbName = nameof(InsurancesApiTests.GetInsuranceById_BadRequestResult);
            var logger = Mock.Of<ILogger<InsurancesController>>();
            using var dbContext = DbContextMocker.GetApplicationDbContext(dbName);      // Disposable!
            var controller = new InsurancesController(dbContext, logger);
            int? findInsuranceID = null;

            // ACT
            IActionResult actionResultGet = controller.GetInsurance(findInsuranceID).Result;

            // ASSERT - check if the IActionResult is BadRequest
            Assert.IsType<BadRequestResult>(actionResultGet);

            // ASSERT - check if the Status Code is (HTTP 400) "BadRequest"
            int expectedStatusCode = (int)System.Net.HttpStatusCode.BadRequest;
            var actualStatusCode = (actionResultGet as BadRequestResult).StatusCode;
            Assert.Equal<int>(expectedStatusCode, actualStatusCode);
        }

        [Fact]
        public void GetInsuranceById_OkResult()
        {
            // ARRANGE
            var dbName = nameof(InsurancesApiTests.GetInsuranceById_OkResult);
            var logger = Mock.Of<ILogger<InsurancesController>>();
            using var dbContext = DbContextMocker.GetApplicationDbContext(dbName);      // Disposable!
            var controller = new InsurancesController(dbContext, logger);
            int findInsuranceID = 2;

            // ACT
            IActionResult actionResultGet = controller.GetInsurance(findInsuranceID).Result;

            // ASSERT - if IActionResult is Ok
            Assert.IsType<OkObjectResult>(actionResultGet);

            // ASSERT - if Status Code is HTTP 200 (Ok)
            int expectedStatusCode = (int)System.Net.HttpStatusCode.OK;
            var actualStatusCode = (actionResultGet as OkObjectResult).StatusCode.Value;
            Assert.Equal<int>(expectedStatusCode, actualStatusCode);
        }

        [Fact]
        public void GetInsuranceById_CorrectResult()
        {
            // ARRANGE
            var dbName = nameof(InsurancesApiTests.GetInsuranceById_OkResult);
            var logger = Mock.Of<ILogger<InsurancesController>>();
            using var dbContext = DbContextMocker.GetApplicationDbContext(dbName);      // Disposable!
            var controller = new InsurancesController(dbContext, logger);
            int findInsuranceID = 2;
            Insurance expectedInsurance = DbContextMocker.TestData_Insurances
                                        .SingleOrDefault(c => c.InsuranceId == findInsuranceID);

            // ACT
            IActionResult actionResultGet = controller.GetInsurance(findInsuranceID).Result;

            // ASSERT - if IActionResult is Ok
            Assert.IsType<OkObjectResult>(actionResultGet);

            // ASSERT - if IActionResult (i.e., OkObjectResult) contains an object of the type Category
            OkObjectResult okResult = actionResultGet.Should().BeOfType<OkObjectResult>().Subject;
            Assert.IsType<Insurance>(okResult.Value);

            // Extract the category object from the result.
            Insurance actualInsurance = okResult.Value.Should().BeAssignableTo<Insurance>().Subject;
            _testOutputHelper.WriteLine($"Found: InsuranceID == {actualInsurance.InsuranceId}");

            // ASSERT - if category is NOT NULL
            Assert.NotNull(actualInsurance);

            // ASSERT - if the CategoryId is containing the expected data.
            Assert.Equal<int>(expected: expectedInsurance.InsuranceId,
                              actual: actualInsurance.InsuranceId);

            // ASSERT - if the CateogoryName is correct 
            Assert.Equal(expected: expectedInsurance.InsuranceName,
                         actual: actualInsurance.InsuranceName);
        }
    }
}

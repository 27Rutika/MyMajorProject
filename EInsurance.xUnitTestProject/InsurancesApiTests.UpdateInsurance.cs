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
        public async void UpdateInsurance_OkResult01()
        {
            // ARRANGE
            var dbName = nameof(InsurancesApiTests.UpdateInsurance_OkResult01);
            var logger = Mock.Of<ILogger<InsurancesController>>();
            using var dbContext = DbContextMocker.GetApplicationDbContext(dbName);      // Disposable!
            var apiController = new InsurancesController(dbContext, logger);
            int editInsuranceId = 2;
            Insurance originalInsurance, changedInsurance;
            changedInsurance = new Insurance
            {
                InsuranceId = editInsuranceId,
                InsuranceName = "--Changed Insurance Name"
            };

            // ACT #1: Get the Record to Edit.

            // (a) Get the Category to edit (to ensure that the row exists before editing it)
            IActionResult actionResultGet = await apiController.GetInsurance(editInsuranceId);

            // (b) Check if HTTP 200 "Ok" 
            OkObjectResult OkResult = actionResultGet.Should().BeOfType<OkObjectResult>().Subject;

            // (c) Extract the Category Object from the OkObjectResult
            originalInsurance = OkResult.Value.Should().BeAssignableTo<Insurance>().Subject;

            // (d) Check if the data to be edited was received from the API
            Assert.NotNull(originalInsurance);

            _testOutputHelper.WriteLine("Retrieved the Data from the API.");

            try
            {
                // ACT #2: Try to update the data, using a completely new object
                //         NOTE: This will throw the InvalidOperationException!
                var actionResultPutAttempt1 = await apiController.PutInsurance(editInsuranceId, changedInsurance);

                // ASSERT - if the update was successfull.
                Assert.IsType<OkResult>(actionResultPutAttempt1);

                _testOutputHelper.WriteLine("Updated the changes back to the API - using a new object.");
            }
            catch (System.InvalidOperationException exp)
            {
                // The PUT operation should throw this exception,
                // because it is an object that does not carry change tracking information.

                _testOutputHelper.WriteLine("Failed to update the change back to the API - using a new object");
                _testOutputHelper.WriteLine($"-- Exception Type: {exp.GetType()}");
                _testOutputHelper.WriteLine($"-- Exception Message: {exp.Message}");
                _testOutputHelper.WriteLine($"-- Exception Source: {exp.Source}");
                _testOutputHelper.WriteLine($"-- Exception TargetSite: {exp.TargetSite}");
            }
        }

        [Fact]
        public async void UpdateInsurance_OkResult02()
        {
            // ARRANGE
            var dbName = nameof(InsurancesApiTests.UpdateInsurance_OkResult02);
            var logger = Mock.Of<ILogger<InsurancesController>>();
            using var dbContext = DbContextMocker.GetApplicationDbContext(dbName);      // Disposable!
            var apiController = new InsurancesController(dbContext, logger);
            int editInsuranceId = 2;
            Insurance originalInsurance;
            string changedInsuranceName = "--- CHANGED Insurance Name";

            // ACT #1: Get the Record to Edit.

            // (a) Get the Category to edit (to ensure that the row exists before editing it)
            IActionResult actionResultGet = await apiController.GetInsurance(editInsuranceId);

            // (b) Check if HTTP 200 "Ok" 
            OkObjectResult OkResult = actionResultGet.Should().BeOfType<OkObjectResult>().Subject;

            // (c) Extract the Category Object from the OkObjectResult
            originalInsurance = OkResult.Value.Should().BeAssignableTo<Insurance>().Subject;

            // (d) Check if the data to be edited was received from the API
            Assert.NotNull(originalInsurance);

            _testOutputHelper.WriteLine("Retrieved the Data from the API.");

            // ACT #2: Now, try to update the data
            // SOLUTION: The following lines would work!
            //           Reason, we are modifying the data originally received.
            //           And then, calling the PUT operation.
            //           So, the API is able to find the ChangeTracking data associated to the object.

            // (a) Change the data of the object that was received from the API.
            originalInsurance.InsuranceName = changedInsuranceName;

            // (b) Call the HTTP PUT API to update the changes (with the updated object)
            var actionResultPutAttempt2 = await apiController.PutInsurance(editInsuranceId, originalInsurance);

            // ASSERT - if the update was successfull.
            Assert.IsType<NoContentResult>(actionResultPutAttempt2);

            _testOutputHelper.WriteLine("Updated the changes back to the API - using the object received");
        }
    }
}

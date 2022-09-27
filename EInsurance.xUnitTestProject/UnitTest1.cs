using System;
using Xunit;
using Xunit.Abstractions;

namespace EInsurance.xUnitTestProject
{
    public class UnitTest1
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public UnitTest1(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void Test1()
        {
            // ARRANGE
            int a = 1, b = 3;
            int expectedResult = 4;
            int actualResult;

            // ACT
            actualResult = a + b;

            _testOutputHelper.WriteLine($"INPUT: a = {a} and b = {b}");
            _testOutputHelper.WriteLine($"RESULT: expected = {expectedResult}, actual = {actualResult}");

            // ASSERT
            Assert.Equal<int>(expectedResult, actualResult);
        }
    }
}

﻿using Api.Controllers;
using Api.Services;
using FluentAssertions;
using Moq;

namespace UTest.Services;

public class BudgetInterestServiceTest
{
    [Theory]
    [InlineData(200_00, 0.04)]
    [InlineData(1_000_000, 0.045)]
    [InlineData(5_000_000, 0.04)]
    public async Task GetInternalInterestRate_WhenDepositIsGreaterOrEqualToZero_OutputFromMocks(double deposit, double expectedRate)
    {
        // arrange
        //
        var ranker = new Mock<IRateRanker>();
        ranker.Setup(r => r.GetRate(It.IsAny<double>())).ReturnsAsync(expectedRate +　0.005);
        var interestService = new BudgetInterestService(ranker.Object);
        
        // act
        //
        var result = await interestService.GetInternalInterestRate(deposit);
        
        // assert
        //
        var tolerance = 0.000_000_1d;
        ranker.Verify(r => r.GetRate(It.IsAny<double>()), Times.Once);
        result.Should().BeApproximately(expectedRate, tolerance);
    }
    
    [Fact]
    public async Task GetInternalInterestRate_WhenDepositIsLessThanZero_ThrowException()
    {
        // arrange
        //
        var expectedMessage = "Deposit must be greater or equal to zero";
        var ranker = new Mock<IRateRanker>();
        ranker.Setup(r => r.GetRate(It.IsAny<double>()));

        var interestService = new BudgetInterestService(ranker.Object);
        
        // act
        //
        var result = interestService.Invoking(s => s.GetInternalInterestRate(-1.0));
        
        // assert
        //
        ranker.Verify(r =>　r.GetRate(It.IsAny<double>()), Times.Never);
        result.Should().ThrowAsync<Exception>().WithMessage(expectedMessage);
    }
}
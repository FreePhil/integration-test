using Api;
using Api.Services;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace ITest.Services;

// [Collection("IntegrationTests")]
public class BudgetInterestServiceTest: IClassFixture<MySqlContainerServiceFixture>
{
    private readonly MySqlContainerServiceFixture mySqlContainer;

    public BudgetInterestServiceTest(MySqlContainerServiceFixture mySqlContainer)
    {
        this.mySqlContainer = mySqlContainer;
    }
    
    [Fact]
    public async Task GetInternalInterestRate_WhenDepositIsLessThanZero_ThrowsException()
    {
        // arrange
        //
        var expectedMessage = "Deposit must be greater or equal to zero";
        var service = new BudgetInterestService(new DbRateRanker(new IntegrationDbContext(mySqlContainer.Options)));

        
        // act
        //
        var result = service.Invoking(s => s.GetInternalInterestRate(-100d));
        
        // assert
        //
        result.Should().ThrowAsync<Exception>().WithMessage(expectedMessage);
    }

    // { 200_000_000, 0.045d},
    // { 50_000_000d, 0.04d},
    // { 20_000_000d, 0.035d},
    // { 0d, 0.03d}
    [Theory]
    [InlineData(0, 0.025d)]
    [InlineData(10_000_000d, 0.025d)]
    [InlineData(19_999_999d, 0.025d)]
    [InlineData(20_000_000d, 0.03d)]
    [InlineData(20_000_001d, 0.03d)]
    [InlineData(49_999_999d, 0.03d)]
    [InlineData(50_000_000d, 0.035d)]
    [InlineData(50_000_001d, 0.035d)]
    [InlineData(199_999_999d, 0.035d)]
    [InlineData(200_000_000d, 0.04d)]
    [InlineData(200_000_001d, 0.04d)]
    public async Task GetInternalInterestRate_WhenDepositIsLessThanZero_ReturnRate(double deposit, double expectedRate)
    {
        // arrange
        //
        var service = new BudgetInterestService(new DbRateRanker(new IntegrationDbContext(mySqlContainer.Options)));

        // act
        //
        var rate = await service.GetInternalInterestRate(deposit);

        // assert
        //
        rate.Should().BeApproximately(expectedRate, 0.000_000_1);
    }
}
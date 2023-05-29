using Api.Services;
using FluentAssertions;

namespace UTest.Services;

public class MemoryRateRankerTest
{
    [Theory]
    [InlineData(300_000_000, 0.045d)]
    [InlineData(200_000_000, 0.045d)]
    [InlineData(100_000_000, 0.04d)]
    [InlineData(50_000_000, 0.04d)]
    [InlineData(30_000_000, 0.035d)]
    [InlineData(20_000_000, 0.035d)]
    [InlineData(5_000_000, 0.03d)]
    [InlineData(0, 0.03d)]
    [InlineData(-1, 0d)]
    public async Task GetRate_WhenDepositIsInTheRange_OutputCorrectRate(double deposit, double expectedRate)
    {
        // arrange
        //
        var ranker = new MemoryRateRanker();

        // act
        //
        var result = await ranker.GetRate(deposit);
        
        // assert
        //
        var tolerance = 0.000_000_001d;
        result.Should().BeApproximately(expectedRate, tolerance);
    }
}
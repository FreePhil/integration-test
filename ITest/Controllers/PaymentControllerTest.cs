using System.Net;
using Api.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;

namespace ITest.Controllers;

public class PaymentControllerTest : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> factory;

    public PaymentControllerTest(WebApplicationFactory<Program> factory)
    {
        this.factory = factory;
    }

    [Theory]
    [InlineData(-1, 0d)]
    public async Task GetInterestRate_WhenDepositIsLessThanZero_ReturnBadRequestWithZeroInterestRate(double deposit, double exptectedRate)
    {
        // arrange
        //
        var client = factory.CreateClient();
        
        // act
        //
        var response = await client.GetAsync($"/payment/{deposit}");
        
        // assert
        //
        var result = JsonConvert.DeserializeObject<InterestRateResponseModel>(await response.Content.ReadAsStringAsync());
        response.StatusCode.Should<HttpStatusCode>().Be(HttpStatusCode.BadRequest);
        result.Rate.Should().BeApproximately(exptectedRate, 0.000_000_1d);
    }

    [Theory]
    [InlineData(100_000_001d, 0d)]
    public async Task GetInterestRate_WhenDepositIsGreaterThen100Million_ReturnOkWithZeroInterestRate(double deposit, double exptectedRate)
    {
        // arrange
        //
        var client = factory.CreateClient();
        
        // act
        //
        var response = await client.GetAsync($"/payment/{deposit}");
        
        // assert
        //
        var result = JsonConvert.DeserializeObject<InterestRateResponseModel>(await response.Content.ReadAsStringAsync());
        response.StatusCode.Should<HttpStatusCode>().Be(HttpStatusCode.OK);
        result.Rate.Should().BeApproximately(exptectedRate, 0.000_000_1d);
    }
    
    [Theory]
    [InlineData(100_000_000d)]
    [InlineData(70_000_000d)]
    [InlineData(33_333_334d)]
    public async Task GetInterestRate_WhenInterestAmountIsGreaterThanOneMillion_ReturnOkWithZeroInterestRate(double deposit)
    {
        // arrange
        //
        var client = factory.CreateClient();
        
        // act
        //
        var response = await client.GetAsync($"/payment/{deposit}");
        
        // assert
        //
        var result = JsonConvert.DeserializeObject<InterestRateResponseModel>(await response.Content.ReadAsStringAsync());
        response.StatusCode.Should<HttpStatusCode>().Be(HttpStatusCode.OK);
        result.Rate.Should().BeApproximately(0d, 0.000_000_1d);
    }
    
    // { 200_000_000, 0.045d },
    // { 50_000_000d, 0.04d },
    // { 20_000_000d, 0.035d },
    // { 0d, 0.03d }
    [Theory]
    [InlineData(0d, 0.025d)]
    [InlineData(5_000_000d, 0.025d)]
    [InlineData(33_333_333d, 0.03d)]
    public async Task GetInterestRate_WhenInterestAmountIsNormal_ReturnOkWithInterestRate(double deposit, double expectedRate)
    {
        // arrange
        //
        var client = factory.CreateClient();
        
        // act
        //
        var response = await client.GetAsync($"/payment/{deposit}");
        
        // assert
        //
        var result = JsonConvert.DeserializeObject<InterestRateResponseModel>(await response.Content.ReadAsStringAsync());
        response.StatusCode.Should<HttpStatusCode>().Be(HttpStatusCode.OK);
        result.Rate.Should().BeApproximately(expectedRate, 0.000_000_1d);
    }
}
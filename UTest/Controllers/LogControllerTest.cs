using System.Xml.XPath;
using Api.Controllers;
using Api.Models;
using Api.Services;
using AutoFixture.Xunit2;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace UTest.Controllers;

public class LogControllerTest
{
    private ILogger<PaymentController> logger;
    public LogControllerTest()
    {
        logger = new NullLogger<PaymentController>();
    }
    
    [Fact]
    public void GetInterestRate_WhenDepositIsNegative_ReturnBadRequest()
    {
        // arrange
        //
        var expectedMessage = "negative deposit is not allowed";
        var logicServiceMock = new Mock<IInterestService>();
        logicServiceMock.Setup(m => m.GetInternalInterestRate(It.IsAny<double>()));
        var controller = new PaymentController(logicServiceMock.Object, logger);

        // act 
        //
        var response = controller.GetInterestRate(-10);

        // assert
        //
        var result = (response as BadRequestObjectResult)!;
        var value = (result.Value as InterestRateResponseModel)!;
        
        logicServiceMock.Verify(d => d.GetInternalInterestRate(It.IsAny<double>()), Times.Never);
        result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        value.Message.Should().StartWith(expectedMessage);
        value.Rate.Should().Be(0.0);
    }
    
    [Fact]
    public void GetInterestRate_WhenDepositIsZero_ReturnOkRequest()
    {
        // arrange
        //
        var expectedMessage = "the rate will change everyday";
        var logicServiceMock = new Mock<IInterestService>();
        logicServiceMock.Setup(m => m.GetInternalInterestRate(It.IsAny<double>()));
        var controller = new PaymentController(logicServiceMock.Object, logger);

        // act 
        //
        var response = controller.GetInterestRate(0);

        // assert
        //
        var result = (response as OkObjectResult)!;
        var value = (result.Value as InterestRateResponseModel)!;
        
        logicServiceMock.Verify(d => d.GetInternalInterestRate(It.IsAny<double>()), Times.Once);
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
        value.Message.Should().StartWith(expectedMessage);
    }
    
    [Fact]
    public void GetInterestRate_WhenDepositIsTooLarge_ReturnOkRequestWithZeroValue()
    {
        // arrange
        //
        var expectedMessage = "please contact us in person";
        var logicServiceMock = new Mock<IInterestService>();
        logicServiceMock.Setup(m => m.GetInternalInterestRate(It.IsAny<double>()));
        var controller = new PaymentController(logicServiceMock.Object, logger);

        // act 
        //
        var response = controller.GetInterestRate(1_000_000_000);

        // assert
        //
        var result = (response as OkObjectResult)!;
        var value = (result.Value as InterestRateResponseModel)!;
        
        logicServiceMock.Verify(d => d.GetInternalInterestRate(It.IsAny<double>()), Times.Never);
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
        value.Message.Should().StartWith(expectedMessage);
        value.Rate.Should().Be(0.0);
    }
    
    [Fact]
    public void GetInterestRate_WhenDepositIs100_000_000AndInterestIsOver1_000_000_ReturnOkRequestWithZeroValue()
    {
        // arrange
        //
        var expectedMessage = "please contact us in person";
        var logicServiceMock = new Mock<IInterestService>();
        logicServiceMock.Setup(m => m.GetInternalInterestRate(It.IsAny<double>())).Returns(0.1);
        
        var controller = new PaymentController(logicServiceMock.Object, logger);

        // act 
        //
        var response = controller.GetInterestRate(100_000_000);

        // assert
        //
        var result = (response as OkObjectResult)!;
        var value = (result.Value as InterestRateResponseModel)!;

        logicServiceMock.Verify(d => d.GetInternalInterestRate(It.IsAny<double>()), Times.Once);
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
        value.Message.Should().StartWith(expectedMessage);
        value.Rate.Should().Be(0.0);
    }
    
    [Fact]
    public void GetInterestRate_WhenDepositIs100_000_000AndInterestIs1_000_000_ReturnOkRequestWithInterest()
    {
        // arrange
        //
        var expectedMessage = "the rate will change everyday";
        var logicServiceMock = new Mock<IInterestService>();
        logicServiceMock.Setup(m => m.GetInternalInterestRate(It.IsAny<double>())).Returns(0.01);
        var controller = new PaymentController(logicServiceMock.Object, logger);

        // act 
        //
        var response = controller.GetInterestRate(100_000_000);

        // assert
        //
        var result = (response as OkObjectResult)!;
        var value = (result.Value as InterestRateResponseModel)!;
        
        logicServiceMock.Verify(d => d.GetInternalInterestRate(It.IsAny<double>()), Times.Once);
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
        value.Message.Should().StartWith(expectedMessage);
    }

    [Theory]
    [InlineData(1_000_000, 0.01, 0.01)]
    [InlineData(5_000_000, 0.05, 0.05)]
    [InlineData(7_000_000, 0.11, 0.11)]
    [InlineData(50.0, 20.0, 20.0)]
    public void GetInterestRate_WhenDepositIsLess100_000_000AndInterestIsLess1_000_000_ReturnOkRequestWithInterestRate(
        double deposit, double rate, double expectedRate)
    {
        // arrange
        //
        var expectedMessage = "the rate will change everyday";
        var logicServiceMock = new Mock<IInterestService>();
        logicServiceMock.Setup(m => m.GetInternalInterestRate(It.IsAny<double>())).Returns(rate);
        var controller = new PaymentController(logicServiceMock.Object, logger);

        // act 
        //
        var response = controller.GetInterestRate(deposit);

        // assert
        //
        var result = (response as OkObjectResult)!;
        var value = (result.Value as InterestRateResponseModel)!;

        logicServiceMock.Verify(d => d.GetInternalInterestRate(It.IsAny<double>()), Times.Once);
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
        value.Message.Should().StartWith(expectedMessage);
        value.Rate.Should().Be(expectedRate);
    }
    
    [Theory]
    [InlineData(10_000_000, 0.12, 0.0)]
    [InlineData(5_000_000, 0.25, 0.0)]
    public void GetInterestRate_WhenDepositIsLess100_000_000AndInterestIsGreater1_000_000_ReturnOkRequestWithZeroRate(
        double deposit, double rate, double expectedRate)
    {
        // arrange
        //
        var expectedMessage = "please contact us in person";
        var logicServiceMock = new Mock<IInterestService>();
        logicServiceMock.Setup(m => m.GetInternalInterestRate(It.IsAny<double>())).Returns(rate);
        var controller = new PaymentController(logicServiceMock.Object, logger);

        // act 
        //
        var response = controller.GetInterestRate(deposit);

        // assert
        //
        var result = (response as OkObjectResult)!;
        var value = (result.Value as InterestRateResponseModel)!;

        logicServiceMock.Verify(d => d.GetInternalInterestRate(It.IsAny<double>()), Times.Once);
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
        value.Message.Should().StartWith(expectedMessage);
        value.Rate.Should().Be(expectedRate);
    }
    
    [Fact]
    public void GetInterestRate_WhenDepositIsUnexpected_ThrowException()
    {
        // arrange
        //
        var logicServiceMock = new Mock<IInterestService>();
        logicServiceMock.Setup(m => m.GetInternalInterestRate(It.IsAny<double>()))
            .Throws(new Exception("unexpected error"));
        var controller = new PaymentController(logicServiceMock.Object, logger);

        // act
        //
        var result = controller.Invoking(p => p.GetInterestRate(10_000.0));
        
        // assert
        //
        logicServiceMock.Verify(d => d.GetInternalInterestRate(It.IsAny<double>()), Times.Never);
        result.Should().Throw<Exception>().WithMessage("unexpected error");
    }
}
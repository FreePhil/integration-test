using Api.Models;
using Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class PaymentController : ControllerBase
{
    private readonly ILogger logger;
    private readonly IInterestService _interestService;

    public PaymentController(IInterestService service, ILogger<PaymentController> logger)
    {
        this.logger = logger;
        this._interestService = service;
    }
    
    [HttpGet]
    public IActionResult GetInterestRate(double deposit)
    {
        if (deposit < 0.0)
        {
            return BadRequest(new InterestRateResponseModel {Message = "negative deposit is not allowed", Rate = 0.0});
        }

        if (deposit > 100_000_000.0)
        {
            return Ok(new InterestRateResponseModel {Message = "please contact us in person", Rate = 0.0});
        }

        var rate = _interestService.GetInternalInterestRate(deposit);

        var interest = deposit * rate;
        if (interest > 1_000_000.0)
        {
            return Ok(new InterestRateResponseModel {Message = "please contact us in person", Rate = 0.0});
        }
        else
        {
            return Ok(new InterestRateResponseModel {Message = "the rate will change everyday", Rate = rate});
        }
    }
}
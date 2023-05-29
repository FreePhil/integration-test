namespace Api.Services;

public interface IInterestService
{
    public Task<double> GetInternalInterestRate(double deposit);
}
namespace Api.Services;

public interface IRateRanker
{
    public string RankerName { get; }
    public Task<double> GetRate(double deposit);
}
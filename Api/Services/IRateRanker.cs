namespace Api.Services;

public interface IRateRanker
{
    public string RankerName { get; }
    public double GetRate(double deposit);
}
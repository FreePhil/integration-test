namespace Api.Services;

public interface IRateRanker
{
    public string RankerName { get; set; }
    public double GetRate(double deposit);
}
namespace Api.Services;

public class MemoryRateRanker: IRateRanker
{
    public string RankerName => nameof(MemoryRateRanker);

    public SortedDictionary<double, double> ranges = new()
    {
        { 200_000_000, 0.045d},
        { 50_000_000d, 0.04d},
        { 20_000_000d, 0.035d},
        { 0d, 0.03d}
    };
    
    public double GetRate(double deposit)
    {
        return 0d;
    }
}
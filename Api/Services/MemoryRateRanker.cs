using System.Security.Cryptography.X509Certificates;

namespace Api.Services;

public class MemoryRateRanker: IRateRanker
{
    public string RankerName => nameof(MemoryRateRanker);
    private static readonly SortedDictionary<double, double> ranges = new()
    {
        { 200_000_000, 0.045d},
        { 50_000_000d, 0.04d},
        { 20_000_000d, 0.035d},
        { 0d, 0.03d}
    };
    
    public Task<double> GetRate(double deposit)
    {
        foreach (var key in ranges.Keys.Reverse())
        {
            if (deposit >= key) return Task.FromResult(ranges[key]);
        }

        return Task.FromResult(0d);
    }
}
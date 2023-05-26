namespace Api.Services;

public class DbRateRanker: IRateRanker
{
    public string RankerName => nameof(DbRateRanker);
    
    public double GetRate(double deposit)
    {
        throw new NotImplementedException();
    }
}
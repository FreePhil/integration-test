using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto.Signers;

namespace Api.Services;

public class DbRateRanker: IRateRanker
{
    private IntegrationDbContext dbContext;
    public string RankerName => nameof(DbRateRanker);

    public DbRateRanker(IntegrationDbContext dbContext)
    {
        this.dbContext = dbContext;
    }
    
    public async Task<double> GetRate(double deposit)
    {
        var ranges = await dbContext.RateRanks.OrderByDescending(r => r.DepositRank).ToListAsync();
        
        foreach (var range in ranges)
        {
            if (deposit >= range.DepositRank) return range.Rate;
        }

        return 0d;
    }
}
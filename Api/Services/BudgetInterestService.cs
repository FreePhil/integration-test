namespace Api.Services;

public class BudgetInterestService: IInterestService
{
    private IRateRanker ranker;
    
    public BudgetInterestService(IRateRanker ranker)
    {
          this.ranker = ranker;
    }
    
    public async Task<double> GetInternalInterestRate(double deposit)
    {
        if (deposit < 0.0d)
        {
            throw new Exception("Deposit must be greater or equal to zero");
        }
        return (await ranker.GetRate(deposit)) - 0.005d;
    }
}
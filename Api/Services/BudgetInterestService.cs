namespace Api.Services;

public class BudgetInterestService: IInterestService
{
    private IRateRanker ranker;
    
    public BudgetInterestService(IRateRanker ranker)
    {
          this.ranker = ranker;
    }
    
    public double GetInternalInterestRate(double deposit)
    {
        if (deposit < 0.0d)
        {
            throw new Exception("Deposit must be greater or equal to zero");
        }
        return ranker.GetRate(deposit) - 0.005d;
    }
}
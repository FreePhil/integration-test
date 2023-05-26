using Api.Services;

namespace Api;

public static class ApiExtension
{
    public static IServiceCollection AddApi(this IServiceCollection services)
    {
        services.AddTransient<IInterestService, BudgetInterestService>();
        services.AddTransient<IRateRanker, MemoryRateRanker>();
        
        return services;
    }
}
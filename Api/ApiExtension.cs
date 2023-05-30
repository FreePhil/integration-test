using Api.Services;

namespace Api;

public static class ApiExtension
{
    public static IServiceCollection AddApi(this IServiceCollection services, ConfigurationManager configuration)
    {
        // setup testcontainer connection string
        //
        configuration.AddEnvironmentVariables();
        var testContainerConnectionString = Environment.GetEnvironmentVariable("TestContainerConnectionString");
        if (!string.IsNullOrEmpty(testContainerConnectionString))
        {
            // Replace the default connection string with the TestContainer connection string
            configuration.AddInMemoryCollection(new Dictionary<string, string>
            {
                {"ConnectionStrings:DefaultConnection", testContainerConnectionString}
            });
        }
        
        services.AddTransient<IInterestService, BudgetInterestService>();
        services.AddTransient<IRateRanker, DbRateRanker>();
        
        return services;
    }
}
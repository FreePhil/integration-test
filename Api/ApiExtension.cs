﻿using Api.Services;

namespace Api;

public static class ApiExtension
{
    public static IServiceCollection AddApi(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddTransient<IInterestService, BudgetInterestService>();
        services.AddTransient<IRateRanker, DbRateRanker>();
        
        return services;
    }
}
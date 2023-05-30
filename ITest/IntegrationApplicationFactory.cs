using Api;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;

namespace ITest;

public class IntegrationApplicationFactory<TProgram>
    : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var connectionString = Environment.GetEnvironmentVariable("TestContainerConnectionString");
            var descriptor =
                services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<IntegrationDbContext>));
            services.Remove(descriptor);
            services.AddDbContext<IntegrationDbContext>(options =>
                options.UseMySQL(connectionString));
        });
    }
}
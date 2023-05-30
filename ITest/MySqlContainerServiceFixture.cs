using Api;
using DotNet.Testcontainers.Builders;
using Microsoft.EntityFrameworkCore;
using Testcontainers.MySql;

namespace ITest;

public class MySqlContainerServiceFixture: IAsyncLifetime
{
    private MySqlContainer container;
    public DbContextOptions<IntegrationDbContext> Options;

    public async Task InitializeAsync()
    {
        container = new MySqlBuilder()
            .WithBindMount(Path.GetFullPath("initsql"), "/docker-entrypoint-initdb.d")
            .WithImage("mysql:8.0.33")
            .WithDatabase("integration")
            .WithUsername("phil")
            .WithPassword("henge")
            .WithExposedPort("3306")
            .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(3306))
            .Build();
        
        await container.StartAsync();
        this.Options = new DbContextOptionsBuilder<IntegrationDbContext>()
            .UseMySQL(container.GetConnectionString())
            .Options;
    }

    public async Task DisposeAsync()
    {
        if (container != null)
        {
            await container.StopAsync();
            await container.DisposeAsync();
        }
    }
}
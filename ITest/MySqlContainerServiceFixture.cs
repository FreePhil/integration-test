using DotNet.Testcontainers.Builders;
using Testcontainers.MySql;

namespace ITest;

public class MySqlContainerServiceFixture: IAsyncLifetime
{
    private MySqlContainer container;

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
        Environment.SetEnvironmentVariable("TestContainerConnectionString", container.GetConnectionString());
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
using DotNet.Testcontainers.Builders;
using FluentAssertions;
using Testcontainers.MySql;

namespace ITest;

public class MySqlContainerPaymentControllerFixture: IAsyncLifetime
{
    private MySqlContainer container;
    public string ConnectionString { get; set; }

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
        ConnectionString = container.GetConnectionString();
        Environment.SetEnvironmentVariable("TestContainerConnectionString", ConnectionString);
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
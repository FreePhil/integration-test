namespace ITest;

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        int i = 0;
        // Retrieve the MySQL TestContainer connection string
    }    
}
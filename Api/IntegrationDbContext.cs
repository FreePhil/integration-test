
using System.Configuration;
using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api;

public class IntegrationDbContext: DbContext
{
    private readonly IConfiguration configuration;
    
    IntegrationDbContext(IConfiguration configuration)
    {
        this.configuration = configuration;
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMySQL(configuration.GetConnectionString("DefaultConnection"));
     }

    public DbSet<RateRank> RateRanks { get; set; }
}
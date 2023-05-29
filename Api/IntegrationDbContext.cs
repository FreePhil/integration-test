
using System.Configuration;
using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api;

public class IntegrationDbContext: DbContext
{
    public DbSet<RateRank> RateRanks { get; set; }
    
    public IntegrationDbContext(DbContextOptions<IntegrationDbContext> options): base(options) 
    {}
}